using System.Collections.Generic;
using System.Threading.Tasks;

namespace xluhco.web.Repositories
{
    public interface IShortLinkRepository
    {
        Task<List<ShortLinkItem>> GetShortLinks();
        Task<ShortLinkItem> GetByShortCode(string shortCode);
    }
}