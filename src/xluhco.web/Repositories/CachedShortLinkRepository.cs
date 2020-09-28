using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace xluhco.web.Repositories
{
    public class CachedShortLinkRepository : IShortLinkRepository
    {
        private List<ShortLinkItem> _shortLinks;
        private readonly ILogger _logger;
        private readonly IShortLinkRepository _repo;

        public CachedShortLinkRepository(ILogger logger, IShortLinkRepository repo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _shortLinks = new List<ShortLinkItem>();
        }

        private async Task PopulateShortLinksIfNone()
        {
            if (_shortLinks.Any())
            {
                return;
            }

            _logger.Warning("No short links in cache -- populating from repo");
            _shortLinks = await _repo.GetShortLinks();
            _logger.Information("Afer populating from cache, there are now {numShortLinks} short links", _shortLinks.Count);
        }

        public async Task<List<ShortLinkItem>> GetShortLinks()
        {
            await PopulateShortLinksIfNone();
            return _shortLinks;
        }

        public async Task<ShortLinkItem> GetByShortCode(string shortCode)
        {
            await PopulateShortLinksIfNone();

            return _shortLinks
                .FirstOrDefault(x => x.ShortLinkCode.Equals(shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}