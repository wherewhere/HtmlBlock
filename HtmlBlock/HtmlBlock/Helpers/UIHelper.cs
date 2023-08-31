using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace HtmlBlock.Helpers
{
    public static class UIHelper
    {
        public static CoreDispatcher TryGetForCurrentCoreDispatcher()
        {
            try
            {
                return CoreApplication.GetCurrentView().Dispatcher;
            }
            catch (Exception)
            {
                return Window.Current?.Dispatcher ?? CoreApplication.MainView.Dispatcher;
            }
        }
    }
}
