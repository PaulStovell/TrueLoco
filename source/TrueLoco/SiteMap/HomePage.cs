using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using TrueLoco.Http;

namespace TrueLoco.SiteMap
{
    public class HomePage : Page
    {
        private string postAction;

        protected override void Interpret(HtmlDocument document)
        {
            var loginForm = document.DocumentNode.CssSelect("#login-form").First();
            postAction = loginForm.GetAttributeValue("action");
        }

        public async Task<ConfirmLogInPage> LogIn(string username, string password)
        {
            var post = new PostDictionary();
            post["j_username"] = username;
            post["j_password"] = password;

            // The confirm page throws if the login is invalid
            return await Browser.Post<ConfirmLogInPage>(postAction, post);
        }
    }
}