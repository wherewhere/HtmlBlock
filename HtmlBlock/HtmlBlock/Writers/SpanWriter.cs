using HtmlAgilityPack;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace HtmlBlock.Writers
{
    public class SpanWriter : HtmlWriter
    {
        public override string[] TargetTags => new string[] { "span" };

        public override DependencyObject GetControl(HtmlNode fragment, TextBlockEx textBlockEx)
        {
            return new Span();
        }
    }
}
