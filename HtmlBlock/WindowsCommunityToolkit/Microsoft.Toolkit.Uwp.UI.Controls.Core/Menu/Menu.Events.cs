// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Menu Control defines a menu of choices for users to invoke.
    /// </summary>
    public partial class Menu
    {
        private const uint AltScanCode = 56;
        private bool _onlyAltCharacterPressed = true;
        private Control _lastFocusElement;
        private bool _isLostFocus = true;
        private Control _lastFocusElementBeforeMenu;
        private Rect _bounds;

        private bool AllowTooltip => (bool)GetValue(AllowTooltipProperty);

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
            _wrapPanel = ItemsPanelRoot as WrapPanel;
            if (_wrapPanel != null)
            {
                _wrapPanel.Orientation = Orientation;
            }

            UpdateMenuItemsFlyoutPlacement();

            Window.Current.CoreWindow.PointerMoved -= CoreWindow_PointerMoved;
            LostFocus -= Menu_LostFocus;
            LostFocus += Menu_LostFocus;
            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.PointerMoved += CoreWindow_PointerMoved;
        }

        private void Menu_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerMoved -= CoreWindow_PointerMoved;
            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;

            // Clear Cache
            foreach (MenuItem menuItem in Items)
            {
                System.Collections.Generic.IEnumerable<MenuFlyoutItemBase> menuFlyoutItems = menuItem.GetMenuFlyoutItems();
                foreach (MenuFlyoutItemBase flyoutItem in menuFlyoutItems)
                {
                    RemoveElementFromCache(flyoutItem);
                }

                RemoveElementFromCache(menuItem);
            }
        }

        private void CoreWindow_PointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            // if contained with the whole Menu control
            if (IsOpened && _bounds.Contains(args.CurrentPoint.Position))
            {
                // if hover over current opened item
                if (SelectedMenuItem.ContainsPoint(args.CurrentPoint.Position))
                {
                    return;
                }

                foreach (MenuItem menuItem in Items)
                {
                    if (menuItem.ContainsPoint(args.CurrentPoint.Position))
                    {
                        SelectedMenuItem.HideMenu();
                        menuItem.Focus(FocusState.Keyboard);
                        menuItem.ShowMenu();
                    }
                }
            }
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (IsInTransitionState)
            {
                return;
            }

            if (NavigateUsingKeyboard(args, this, Orientation))
            {
                return;
            }

            string gestureKey = MapInputToGestureKey(args.VirtualKey);

            if (gestureKey == null)
            {
                return;
            }

            if (MenuItemInputGestureCache.ContainsKey(gestureKey))
            {
                DependencyObject cachedMenuItem = MenuItemInputGestureCache[gestureKey];
                if (cachedMenuItem is MenuFlyoutItem menuFlyoutItem)
                {
                    menuFlyoutItem.Command?.Execute(menuFlyoutItem.CommandParameter);
                }
            }
        }

        private void Menu_LostFocus(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = ControlHelpers.IsXamlRootAvailable && XamlRoot != null
                ? FocusManager.GetFocusedElement(XamlRoot) as MenuItem
                : FocusManager.GetFocusedElement() as MenuItem;
            if (AllowTooltip)
            {
                HideMenuItemsTooltips();
            }

            if (menuItem != null || IsOpened)
            {
                return;
            }

            _isLostFocus = true;
            if (!AllowTooltip)
            {
                RemoveUnderlineMenuItems();
            }
        }

        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (Items.Count == 0)
            {
                return;
            }

            _lastFocusElement = ControlHelpers.IsXamlRootAvailable && XamlRoot != null
                ? FocusManager.GetFocusedElement(XamlRoot) as Control
                : FocusManager.GetFocusedElement() as Control;

            if (args.KeyStatus.ScanCode != AltScanCode)
            {
                _onlyAltCharacterPressed = false;
            }

            if (args.VirtualKey == VirtualKey.Menu)
            {
                if (!IsOpened)
                {
                    if (_isLostFocus)
                    {
                        if (_onlyAltCharacterPressed && args.KeyStatus.IsKeyReleased)
                        {
                            ((MenuItem)Items[0]).Focus(FocusState.Programmatic);

                            if (!(_lastFocusElement is MenuItem))
                            {
                                _lastFocusElementBeforeMenu = _lastFocusElement;
                            }
                        }

                        if (AllowTooltip)
                        {
                            ShowMenuItemsToolTips();
                        }
                        else
                        {
                            UnderlineMenuItems();
                        }

                        if (args.KeyStatus.IsKeyReleased)
                        {
                            _isLostFocus = false;
                        }
                    }
                    else if (args.KeyStatus.IsKeyReleased)
                    {
                        HideToolTip();
                        _lastFocusElementBeforeMenu?.Focus(FocusState.Keyboard);
                    }
                }
            }
            else if ((args.KeyStatus.IsMenuKeyDown || !_isLostFocus) && args.KeyStatus.IsKeyReleased)
            {
                string gestureKey = MapInputToGestureKey(args.VirtualKey, !_isLostFocus);
                if (gestureKey == null)
                {
                    return;
                }

                if (MenuItemInputGestureCache.ContainsKey(gestureKey))
                {
                    DependencyObject cachedMenuItem = MenuItemInputGestureCache[gestureKey];
                    if (cachedMenuItem is MenuItem menuItem)
                    {
                        menuItem.ShowMenu();
                        menuItem.Focus(FocusState.Keyboard);
                    }
                }
            }

            if (args.KeyStatus.IsKeyReleased && args.EventType == CoreAcceleratorKeyEventType.KeyUp)
            {
                _onlyAltCharacterPressed = true;
                _isLostFocus = true;
                HideToolTip();
            }
        }

        private void HideToolTip()
        {
            if (AllowTooltip)
            {
                HideMenuItemsTooltips();
            }
            else
            {
                RemoveUnderlineMenuItems();
            }
        }
    }
}