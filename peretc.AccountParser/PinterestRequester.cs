using System.Text;
using Newtonsoft.Json.Linq;

namespace peretc.AccountParser
{
    class PinterestRequester
    {
        public static JObject GetResponceContentObj(HttpResponseMessage response)
        {
            var contentBytes = response.Content.ReadAsByteArrayAsync().Result;
            var contentStr = Encoding.UTF8.GetString(contentBytes);
            dynamic contentJson = JObject.Parse(contentStr);
            return contentJson;
        }
        HttpClient httpClient;

        public PinterestRequester(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<HttpResponseMessage> MakeRequest(PinterestRequestData data)
        {
            return MakeRequest(data.baseUrl, data.options, data.sourceUrl).Result;
        }
        public async Task<HttpResponseMessage> MakeRequest(
            string baseUrl,
            object options,
            string sourceUrl)
        {
            var client = httpClient;
            client.AddHeadersFromText(Encoding.UTF8.GetString(ParseBody.RequestHeaders_bulk));

            var finalUrl = baseUrl
                .AddQueryParams(new Dictionary<string, string>
                {
                    ["source_url"] = sourceUrl
                })
                .AddJsonQueryParam("data", new { options, context = new { } });

            var response = await client.GetAsync(finalUrl);
            return response;
            // Поддержка вложенных свойств через путь (например, "resource_response.data")

        }
    }
}
