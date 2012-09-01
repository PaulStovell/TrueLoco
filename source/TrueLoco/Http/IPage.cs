using HtmlAgilityPack;

namespace TrueLoco.Http
{
    public interface IPage
    {
        void Load(BrowserSession browser, HtmlDocument document);
    }
}