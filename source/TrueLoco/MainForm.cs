using System;
using System.Linq;
using System.Windows.Forms;
using TrueLoco.Http;
using TrueLoco.SiteMap;

namespace TrueLoco
{
    public partial class MainForm : Form
    {
        private BrowserSession session;

        public MainForm()
        {
            InitializeComponent();
        }

        private async void LogInClicked(object sender, EventArgs e)
        {
            BeginBusy("Loading...");

            try
            {
                session = new BrowserSession();
            
                Status("Loading home page...");
                var homePage = await session.Get<HomePage>("http://www.truelocal.com.au/");

                Status("Logging in...");
                var accountPage = await homePage.LogIn(usernameText.Text, passwordText.Text);

                Status("Fetching business listings...");
                var listingsPage = await accountPage.GoToListings();

                foreach (var listing in listingsPage.Listings)
                {
                    Status("Getting listing " + listing.BusinessName + "...");

                    await listingsPage.LoadListingDetails(listing);
                }

                mobilePhone.Text = listingsPage.Listings.First().MobilePhoneNumber;
            }
            finally
            {
                EndBusy();
            }
        }

        void BeginBusy(string text)
        {
            Enabled = false;
            Status(text);
            progressBar.Visible = true;
            progressLabel.Visible = true;
        }

        void Status(string text)
        {
            progressLabel.Text = text;
        }

        void EndBusy()
        {
            progressBar.Visible = false;
            progressLabel.Visible = false;
            Enabled = true;
        }
    }
}
