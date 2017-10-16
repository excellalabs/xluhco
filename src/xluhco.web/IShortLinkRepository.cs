using System.Collections.Generic;

namespace xluhco.web
{
    public interface IShortLinkRepository
    {
        List<ShortLinkItem> GetShortLinks();
        ShortLinkItem GetByShortCode(string shortCode);
    }
}