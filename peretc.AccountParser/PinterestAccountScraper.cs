using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace peretc.AccountParser
{
    public partial class PinterestAccountScraper
    {
        readonly string _username;
        const string BoardsResourceLink = "https://ru.pinterest.com/resource/BoardsResource";
        const string BoardFeedResourceLink = "https://ru.pinterest.com/resource/BoardFeedResource";

        JArray _userBoardsCache = new JArray();
        Dictionary<BoardData, JArray> _boardsPinsCache = new Dictionary<BoardData, JArray>();

        public PinterestAccountScraper(string username)
        {
            _username = username;
        }
        HttpClient NewPinterestClient()
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Brotli
            };
            return new HttpClient(handler);
        }
        public async Task Cache()
        {
            var boards = await GetBoardList();
            foreach (var item in boards)
                await GetBoardPins(item);
        }
        public abstract class BookmarkedOptions{
            public Bookmark Bookmark { get; protected set; }
            public object Options { get; protected init;}
        }
        class UserBoardsRequestOptions : BookmarkedOptions
        {
            public UserBoardsRequestOptions(string username)
            {
                var options = new
                {
                    username = username,
                    privacy_filter = "all",
                    sort = "last_pinned_to",
                    field_set_key = "profile_grid_item",
                    filter_stories = false,
                    page_size = 250,
                    bookmarks = new List<string> { },
                    group_by = "mix_public_private",
                    include_archived = true,
                    redux_normalize_feed = true,
                    filter_all_pins = false
                };

                Options = options;
                Bookmark = new Bookmark(options.bookmarks);
            }
        }


        static partial class PinterestRequestDataStatic
        {
            public static PinterestRequestData UserBoards(string username)
            {
                var options = new UserBoardsRequestOptions(username);
                var result = new PinterestRequestData()
                {
                    baseUrl = BoardsResourceLink.GET(),
                    options = options.Options,
                    sourceUrl = username
                };
                result.BookmarkChange += (bm) => options.Bookmark.Value = bm;
                return result;
            }
        }
        public async Task<BoardData[]> GetBoardList()
        {
            var boards = new List<BoardData>();

            if (_userBoardsCache.Count != 0)
                return convertJArray(_userBoardsCache);

            var requestData = PinterestRequestDataStatic.UserBoards(_username); new PinterestRequestData();

            BoardData[] convertJArray(JArray jArray)
            {
                return jArray.Select(d => new BoardData
                {
                    Id = (string)d["id"],
                    Name = (string)d["name"]
                }).ToArray();
            }

            void Scrapper_ResponceRevieved(HttpResponseMessage obj)
            {
                var responseContentObj =PinterestRequester.GetResponceContentObj(obj);
                JArray boardsArray = Get(responseContentObj, "resource_response.data");
                foreach (var item in boardsArray)
                    _userBoardsCache.Add(item);
                boards.AddRange(convertJArray(boardsArray));
            }

            var scrapper = new PortionScraper(NewPinterestClient());
            scrapper.ResponceRecieved += Scrapper_ResponceRevieved;
            _userBoardsCache.Clear();
            await scrapper.Scrap(requestData);

            return boards.ToArray();
        }

        private dynamic Get(JObject jsonObj, string dataPath)
        {
            dynamic data = jsonObj;
            foreach (var part in dataPath.Split('.'))
            {
                data = data[part];
            }

            return data;
        }
        class BoardPinsOptions : BookmarkedOptions
        {
            public BoardPinsOptions(string username,BoardData boardData)
            {
                var boardUrl = HttpUtility.UrlEncode($"/{username}/{boardData.Name}/");
                var options = new
                {
                    board_id = boardData.Id,
                    board_url = boardUrl,
                    currentFilter = -1,
                    field_set_key = "partner_react_grid_pin",
                    filter_section_pins = true,
                    sort = "default",
                    layout = "default",
                    bookmarks = new List<string> { },
                    page_size = 250,
                    redux_normalize_feed = true
                };

                Options = options;
                Bookmark = new Bookmark(options.bookmarks);
            }
        }
        static partial class PinterestRequestDataStatic
        {
            public static PinterestRequestData BoardPins(string username, BoardData boardData)
            {
                var options = new BoardPinsOptions(username,boardData);
                var boardUrl = HttpUtility.UrlEncode($"/{username}/{boardData.Name}/");
                var result = new PinterestRequestData()
                {
                    baseUrl = BoardFeedResourceLink.GET(),
                    options = options.Options,
                    sourceUrl = boardUrl
                };
                result.BookmarkChange += (bm) => options.Bookmark.Value = bm;
                return result;
            }
        }
        public async Task<PinData[]> GetBoardPins(BoardData boardData)
        {
            List<PinData> pins = new List<PinData>();

            if (_boardsPinsCache.TryGetValue(boardData,out var jArray))
                return convertJArray(jArray);

            var requestData = PinterestRequestDataStatic.BoardPins(_username,boardData);

            PinData[] convertJArray(JArray jArray)
            {
                return jArray.Select(d => new PinData
                {
                    Description = (string)d["description"],
                    Id = (string)d["id"]
                }).ToArray();
            }

            void Scrapper_ResponceRevieved(HttpResponseMessage obj)
            {
                var responseContentObj = PinterestRequester.GetResponceContentObj(obj);
                JArray pinsArray = Get(responseContentObj, "resource_response.data");
                if (_boardsPinsCache.ContainsKey(boardData))
                    foreach (var item in pinsArray)
                        _boardsPinsCache[boardData].Add(item);
                else
                    _boardsPinsCache[boardData] = pinsArray;

                pins.AddRange(convertJArray(pinsArray));
            }

            var scrapper = new PortionScraper(NewPinterestClient());
            scrapper.ResponceRecieved += Scrapper_ResponceRevieved;
            scrapper.Scrap(requestData);

            return pins.ToArray();
        }


    }
}
