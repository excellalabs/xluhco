namespace xluhco.web
{
    public class ShortLinkItem : IShortLinkItem
    {
        public string ShortLinkCode { get; }
        public string URL { get; }

        public ShortLinkItem(string shortLinkCode, string url)
        {
            ShortLinkCode = shortLinkCode;
            URL = url;
        }
    }
}
