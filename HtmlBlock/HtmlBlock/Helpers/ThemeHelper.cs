using HtmlBlock.Common;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace HtmlBlock.Helpers
{
    /// <summary>
    /// Class providing functionality around switching and restoring theme settings
    /// </summary>
    public static class ThemeHelper
    {
        // Keep reference so it does not get optimized/garbage collected
        public static UISettings UISettings;

        public static WeakEvent<UISettingChangedType> UISettingChanged { get; } = new WeakEvent<UISettingChangedType>();

        static ThemeHelper()
        {
            // Registering to color changes, thus we notice when user changes theme system wide
            UISettings = new UISettings();
            UISettings.ColorValuesChanged += UISettings_ColorValuesChanged;
        }

        private static void UISettings_ColorValuesChanged(UISettings sender, object args)
        {
            UISettingChanged.Invoke(IsDarkTheme() ? UISettingChangedType.DarkMode : UISettingChangedType.LightMode);
        }

        public static bool IsDarkTheme()
        {
            return UISettings?.GetColorValue(UIColorType.Background) == Colors.Black;
        }
    }

    public enum UISettingChangedType
    {
        LightMode,
        DarkMode,
        NoPicChanged,
    }
}
