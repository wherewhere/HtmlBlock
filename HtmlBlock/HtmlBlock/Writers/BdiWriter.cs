using HtmlAgilityPack;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HtmlBlock.Writers
{
    public class BdiWriter : HtmlWriter
    {
        public override string[] TargetTags => new string[] { "bdi" };

        public override DependencyObject GetControl(HtmlNode fragment, TextBlockEx textBlockEx)
        {
            return new RichTextBlock();
        }
    }
}
