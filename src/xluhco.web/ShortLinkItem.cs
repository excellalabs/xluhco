namespace xluhco.web
{
    public class ShortLinkItem : IShortLinkItem
    {
        public string ShortLinkCode { get; }
        public string URL { get; }
        public string ImageURL { get; }
        public string Title { get; }
        public string Description { get; }

        public ShortLinkItem(string shortLinkCode, string url, string imageURL = null, string title = null,
            string description = null)
        {
            ShortLinkCode = shortLinkCode;
            URL = url;
            Title = title;
            Description = description;
            ImageURL = imageURL;
        }
    }
}
