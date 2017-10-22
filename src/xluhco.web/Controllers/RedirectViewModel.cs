namespace xluhco.web.Controllers
{
    public class RedirectViewModel
    {
        public RedirectViewModel(string trackingCode, int numSecondsToWait, string shortLinkCode, string url)
        {
            TrackingCode = trackingCode;
            NumberOfSecondsToWait = numSecondsToWait;
            ShortLinkCode = shortLinkCode;
            Url = url;
        }

        public string TrackingCode { get; }
        public int NumberOfSecondsToWait { get; }
        public string ShortLinkCode { get; }
        public string Url { get; }
    }
}