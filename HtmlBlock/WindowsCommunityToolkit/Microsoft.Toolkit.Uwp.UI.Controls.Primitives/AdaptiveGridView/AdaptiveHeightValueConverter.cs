// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class AdaptiveHeightValueConverter : IValueConverter
    {
        private Thickness thickness = new Thickness(0, 0, 4, 4);

        public Thickness DefaultItemMargin
        {
            get { return thickness; }
            set { thickness = value; }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                GridView gridView = (GridView)parameter;
                if (gridView == null)
                {
                    return value;
                }

                double.TryParse(value.ToString(), out double height);

                Thickness padding = gridView.Padding;
                Thickness margin = GetItemMargin(gridView, DefaultItemMargin);
                height = height + margin.Top + margin.Bottom + padding.Top + padding.Bottom;

                return height;
            }

            return double.NaN;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        internal static Thickness GetItemMargin(GridView view, Thickness fallback = default)
        {
            Setter setter = view.ItemContainerStyle?.Setters.OfType<Setter>().FirstOrDefault(s => s.Property == FrameworkElement.MarginProperty);
            if (setter != null)
            {
                return (Thickness)setter.Value;
            }
            else
            {
                if (view.Items.Count > 0)
                {
                    GridViewItem container = (GridViewItem)view.ContainerFromIndex(0);
                    if (container != null)
                    {
                        return container.Margin;
                    }
                }

                // Use the default thickness for a GridViewItem
                return fallback;
            }
        }
    }
}