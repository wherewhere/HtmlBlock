using HtmlAgilityPack;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace HtmlBlock.Writers
{
    public class ParagraphWriter : HtmlWriter
    {
        public override string[] TargetTags => new string[] { "p" };

        public override DependencyObject GetControl(HtmlNode fragment, TextBlockEx textBlockEx)
        {
            return new Paragraph();
        }
    }
}
