using HtmlAgilityPack;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace HtmlBlock.Writers
{
    public class InsWriter : HtmlWriter
    {
        public override string[] TargetTags => new string[] { "ins", "u" };

        public override DependencyObject GetControl(HtmlNode fragment, TextBlockEx textBlockEx)
        {
            return new Underline();
        }
    }
}
