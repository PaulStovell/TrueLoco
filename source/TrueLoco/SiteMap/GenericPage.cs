using HtmlAgilityPack;
using TrueLoco.Http;

namespace TrueLoco.SiteMap
{
    public class GenericPage : Page
    {
        public HtmlDocument Document { get; private set; }
        
        protected override void Interpret(HtmlDocument document)
        {
            Document = document;
        }
    }
}