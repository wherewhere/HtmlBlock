using HtmlAgilityPack;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace HtmlBlock.Writers
{
    public class StrongWriter : HtmlWriter
    {
        public override string[] TargetTags => new string[] { "strong", "b" };

        public override DependencyObject GetControl(HtmlNode fragment, TextBlockEx textBlockEx)
        {
            return new Bold();
        }
    }
}
