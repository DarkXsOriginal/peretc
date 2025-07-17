namespace peretc.AccountParser
{
    public struct PinterestRequestData
    {
        public void SetBookmark(string bookmark) => BookmarkChange.Invoke(bookmark);
        public event Action<string> BookmarkChange;
        public string baseUrl;
        public object options;
        public string sourceUrl;
    }
}
