using System.Collections.Generic;

namespace xluhco.web.Repositories
{
    public interface IShortLinkRepository
    {
        List<ShortLinkItem> GetShortLinks();
        ShortLinkItem GetByShortCode(string shortCode);
    }
}