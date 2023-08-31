using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace HtmlBlock.Containers
{
    public class ParagraphDocumentContainer : DocumentContainer<Paragraph>
    {
        public ParagraphDocumentContainer(Paragraph element) : base(element)
        {
        }

        public override bool CanContain(DependencyObject element)
        {
            return element is Inline;
        }

        protected override void Add(DependencyObject element)
        {
            Inline inline = element as Inline;
            Control.Inlines.Add(inline);
        }
    }
}
