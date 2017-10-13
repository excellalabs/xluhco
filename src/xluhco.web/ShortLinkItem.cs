using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xluhco.web
{
    public interface IShortLinkItem
    {
        string ShortLinkCode { get; }
        string URL { get; }
    }

    public class ShortLinkItem : IShortLinkItem
    {
        public string ShortLinkCode { get; private set; }
        public string URL { get; private set; }

        public ShortLinkItem(string shortLinkCode, string url)
        {
            this.ShortLinkCode = shortLinkCode;
            this.URL = url;
        }
    }

    public interface IShortLinkRepository
    {
        List<ShortLinkItem> GetShortLinks();
        ShortLinkItem GetByShortCode(string shortCode);
    }

    public class CachedShortLinkFromCsvRepository : IShortLinkRepository
    {
        private readonly List<ShortLinkItem> _shortLinks;

        public CachedShortLinkFromCsvRepository()
        {
            _shortLinks = new List<ShortLinkItem>()
            {
                new ShortLinkItem("sk", "http://SeanKilleen.com"),
                new ShortLinkItem("skpres", "http://SeanKilleen.com/Presentations"),
                new ShortLinkItem("du", "http://TheJavaScriptPromise.com"),
            };

        }

        private void PopulateShortLinks()
        {
            //TODO: Implement piece that reads from CSV
        }

        public List<ShortLinkItem> GetShortLinks()
        {
            return _shortLinks; //TODO: populate if empty
        }

        public ShortLinkItem GetByShortCode(string shortCode)
        {
            return _shortLinks
                .FirstOrDefault(x => x.ShortLinkCode.Equals(shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }

}
