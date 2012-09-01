using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using TrueLoco.Http;

namespace TrueLoco.SiteMap
{
    public class ManageListingsPage : Page
    {
        public List<BusinessListingRecord> Listings { get; private set; } 

        protected override void Interpret(HtmlDocument document)
        {
            var listings = new List<BusinessListingRecord>();

            foreach (var listingDiv in document.DocumentNode.CssSelect("ol.business-listings li.listing-open"))
            {
                var listing = new BusinessListingRecord();
                listing.BusinessName = listingDiv.CssSelect("li.bl-name strong").First().InnerText;
                listing.OrganisationId = listingDiv.CssSelect("input").First(i => i.GetAttributeValue("name") == "organisationId").GetAttributeValue("value");
                listing.ContactDetailsHref = listingDiv.CssSelect(".listing-quicknav li.contact div strong a").First().GetAttributeValue("href");
                listings.Add(listing);
            }

            Listings = listings;
        }

        public async Task LoadListingDetails(BusinessListingRecord listingRecord)
        {
            var contactPage = await Browser.Get<GenericPage>(listingRecord.ContactDetailsHref);
            listingRecord.MobilePhoneNumber = contactPage.Document.DocumentNode.CssSelect("fieldset.contact-info input").First(i => i.GetAttributeValue("name", "") == "mobileNumber").GetAttributeValue("value");
        }
    }
}