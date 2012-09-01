using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace TrueLoco.Http
{
    public class BrowserSession
    {
        readonly CookieContainer cookies = new CookieContainer();
        private Uri lastUri;

        public async Task<TPage> Get<TPage>(string urlOrPath) where TPage : IPage, new()
        {
            return await ExecuteRequest<TPage>(urlOrPath, "GET", null);
        }

        public async Task<TPage> Post<TPage>(string urlOrPath, PostDictionary postData) where TPage : IPage, new()
        {
            return await ExecuteRequest<TPage>(urlOrPath, "POST", postData);
        }

        async Task<TPage> ExecuteRequest<TPage>(string urlOrPath, string method, PostDictionary postData) where TPage : IPage, new()
        {
            var uri = ResolveUri(urlOrPath);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.PreAuthenticate = false;
            request.Method = method;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.83 Safari/537.1";
            request.CookieContainer = cookies;

            if (postData != null)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                
                var post = EncodeFormData(postData);
                var requestStream = await request.GetRequestStreamAsync();
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(post);
                }
            }

            var response = (HttpWebResponse)await request.GetResponseAsync();

            HtmlDocument document;
            using (var stream = response.GetResponseStream())
            {
                document = new HtmlDocument();
                document.Load(stream);
            }

            var page = new TPage();
            page.Load(this, document);
            return page;
        }

        static string EncodeFormData(PostDictionary postData)
        {
            var pairs =
                from name in postData.Keys
                let value = postData[name]
                let valueAsString = (value ?? string.Empty)
                let encoded = HttpUtility.UrlEncode(valueAsString)
                select name + "=" + value;

            return string.Join("&", pairs);
        }

        Uri ResolveUri(string uri)
        {
            if (lastUri == null)
            {
                return lastUri = new Uri(uri, UriKind.Absolute);
            }

            Uri absolute;
            if (Uri.TryCreate(uri, UriKind.Absolute, out absolute))
            {
                return lastUri = absolute;
            }

            return lastUri = new Uri(lastUri, uri);
        }
    }
}