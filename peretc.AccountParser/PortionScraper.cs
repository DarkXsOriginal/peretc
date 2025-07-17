using Newtonsoft.Json.Linq;

namespace peretc.AccountParser
{
    public class PortionScraper
    {
        HttpClient httpClient;

        public PortionScraper(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public event Action<HttpResponseMessage> ResponceRecieved;
        private string GetBookmark(JObject responseContentObj)
        {
            return (string)responseContentObj["resource"]["options"]["bookmarks"][0];
        }
        public async Task Scrap(PinterestRequestData requestData)
        {
            PinterestRequester requester = new PinterestRequester(httpClient);
            string bookmarkValue;
            do
            {
                var response = await requester.MakeRequest(requestData);

                JObject responseContentObj = PinterestRequester.GetResponceContentObj(response);
                bookmarkValue = GetBookmark(responseContentObj);
                requestData.SetBookmark(bookmarkValue);
                ResponceRecieved?.Invoke(response);
            } while (bookmarkValue != "-end-");
        }
    }
}
