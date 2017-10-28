using System.Collections.Generic;
namespace xluhco.web
{
    public class ListPageViewModel
    {
        public List<ShortLinkItem> ShortLinkList  { get; }
        public SiteOptions SiteOptions { get; }

        public ListPageViewModel(List<ShortLinkItem> links, SiteOptions options)
        {
            ShortLinkList = links;
            SiteOptions = options;
        }
    }
}

