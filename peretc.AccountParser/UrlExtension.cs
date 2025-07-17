using Newtonsoft.Json;

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
        public static string AddJsonQueryParam(this string baseUrl, string paramName, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            //var encodedJson = HttpUtility.UrlEncode(json);

            return baseUrl.AddQueryParams(new Dictionary<string, string>
            {
                [paramName] = json
            });
        }
    }
}
