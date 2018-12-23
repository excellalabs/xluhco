namespace xluhco.web.Controllers
{
    public class RedirectViewModel
    {
        public RedirectViewModel(string trackingCode, int numSecondsToWait, ShortLinkItem shortLinkItem)
        {
            TrackingCode = trackingCode;
            NumberOfSecondsToWait = numSecondsToWait;
            ShortLinkCode = shortLinkItem.ShortLinkCode;
            Url = shortLinkItem.URL;
            Title = shortLinkItem.Title;
            Description = shortLinkItem.Description;
            ImageUrl = shortLinkItem.ImageURL;
        }

        public string TrackingCode { get; }
        public int NumberOfSecondsToWait { get; }
        public string ShortLinkCode { get; }
        public string Url { get; }
        public string Title { get; }
        public string Description { get; }
        public string ImageUrl { get; }
    }
}