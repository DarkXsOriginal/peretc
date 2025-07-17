namespace peretc.AccountParser
{
    public class Bookmark
    {
        public string Value
        {
            get => _bookmarks.FirstOrDefault(); set
            {
                _bookmarks.Clear();
                _bookmarks.Add(value);
            }
        }
        List<string> _bookmarks = new List<string>() { string.Empty };

        public Bookmark(List<string> bookmarks)
        {
            _bookmarks = bookmarks;
        }
    }
}
