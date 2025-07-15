using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace peretc.AccountParser
{
    static class UrlExtension
    {
        public static string GET(this string link) => link + "/get";
        public static string AddQueryParams(this string baseUrl, Dictionary<string, string> parameters)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var param in parameters)
            {
                query[param.Key] = param.Value;
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }

    public class AccountParser
    {
        readonly string Username;
        const string BoardsResourceLink = "https://ru.pinterest.com/resource/BoardsResource";

        public AccountParser(string username)
        {
            Username = username;
        }
        public async Task<string[]> GetBoardList()
        {
            List<string> boardNames = new List<string>();
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Brotli
            };
            var link = BoardsResourceLink.GET();
            var client = new HttpClient(handler);
            
            client.AddHeadersFromText(Encoding.UTF8.GetString(ParseBody.RequestHeaders_bulk));
            var query_params = new Dictionary<string, string>() {
                {"source_url",Username},
                {"data", Encoding.UTF8.GetString(ParseBody.RequestParams_json) }
                };
            var url = link.AddQueryParams(query_params);
            var response = client.GetAsync(url).Result;
            var content = response.Content;
            var content_bytes = content.ReadAsByteArrayAsync().Result;
            var content_str = Encoding.UTF8.GetString(content_bytes);
            var type = response.Content.Headers.ContentType?.MediaType;
            dynamic content_json = JObject.Parse(content_str);
            JArray data = content_json.resource_response.data;
            foreach (JObject d in data)
            {
                var name = ((string)d["name"]);
                boardNames.Add(name);
            }
            return boardNames.ToArray();
        }
    }
}
