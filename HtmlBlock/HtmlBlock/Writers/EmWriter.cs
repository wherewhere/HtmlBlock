using HtmlAgilityPack;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace HtmlBlock.Writers
{
    public class EmWriter : HtmlWriter
    {
        public override string[] TargetTags => new string[] { "em", "i", "dfn", "var", "cite", "address" };

        public override DependencyObject GetControl(HtmlNode fragment, TextBlockEx textBlockEx)
        {
            return new Italic();
        }
    }
}
