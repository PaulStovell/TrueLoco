using HtmlAgilityPack;

namespace TrueLoco.Http
{
    public abstract class Page : IPage
    {
        protected BrowserSession Browser { get; private set; }

        protected abstract void Interpret(HtmlDocument document);

        void IPage.Load(BrowserSession browser, HtmlDocument document)
        {
            Browser = browser;
            Interpret(document);
        }
    }
}