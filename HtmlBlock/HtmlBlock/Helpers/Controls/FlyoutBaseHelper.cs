using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace HtmlBlock.Helpers
{
    public static class FlyoutBaseHelper
    {
        #region ShouldConstrainToRootBounds

        /// <summary>
        /// Gets a value that indicates whether the flyout should be shown within the bounds of the XAML root.
        /// </summary>
        /// <param name="control">The element from which to read the property value.</param>
        /// <returns>A value that indicates whether the flyout should be shown within the bounds of the XAML root.</returns>
        public static bool GetShouldConstrainToRootBounds(FlyoutBase control)
        {
            return (bool)control.GetValue(ShouldConstrainToRootBoundsProperty);
        }

        /// <summary>
        /// Sets a value that indicates whether the flyout should be shown within the bounds of the XAML root.
        /// </summary>
        /// <param name="control">The element on which to set the attached property.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetShouldConstrainToRootBounds(FlyoutBase control, bool value)
        {
            control.SetValue(ShouldConstrainToRootBoundsProperty, value);
        }

        /// <summary>
        /// Identifies the ShouldConstrainToRootBounds dependency property.
        /// </summary>
        public static readonly DependencyProperty ShouldConstrainToRootBoundsProperty =
            DependencyProperty.RegisterAttached(
                "ShouldConstrainToRootBounds",
                typeof(bool),
                typeof(FlyoutBaseHelper),
                new PropertyMetadata(true, OnShouldConstrainToRootBoundsChanged));

        private static void OnShouldConstrainToRootBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlyoutBase flyout)
            {
                if (e.NewValue is bool ShouldConstrainToRootBounds && ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Controls.Primitives.FlyoutBase", "ShouldConstrainToRootBounds"))
                {
                    flyout.ShouldConstrainToRootBounds = ShouldConstrainToRootBounds;
                }
            }
        }

        #endregion
    }
}
