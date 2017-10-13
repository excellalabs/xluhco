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
}
