using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using TrueLoco.Http;

namespace TrueLoco.SiteMap
{
    public class ConfirmLogInPage : Page
    {
        protected override void Interpret(HtmlDocument document)
        {
            // Ensure login was successful
            var error = document.DocumentNode.CssSelect("ul.errorMessage li span").FirstOrDefault();
            if (error != null)
            {
                throw new Exception("Error: " + error.InnerText);
            }
        }

        public async Task<ManageListingsPage> GoToListings()
        {
            // After successful login, we should be able to visit the home page to fetch the link to our profile
            var home = await Browser.Get<GenericPage>("/");
            var listingsLink = home.Document.DocumentNode.CssSelect("a.menu-business-listing").FirstOrDefault(a => a.InnerText.IndexOf("Manage listing", StringComparison.OrdinalIgnoreCase) >= 0);
            var href = listingsLink.GetAttributeValue("href");
            return await Browser.Get<ManageListingsPage>(href);
        }
    }
}