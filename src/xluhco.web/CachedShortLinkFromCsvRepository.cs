using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace xluhco.web
{
    public class CachedShortLinkFromCsvRepository : IShortLinkRepository
    {
        private List<ShortLinkItem> _shortLinks;
        private readonly ILogger _logger;
        private readonly IShortLinkRepository _repo;

        public CachedShortLinkFromCsvRepository(ILogger logger, IShortLinkRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _shortLinks = new List<ShortLinkItem>();
        }

        private void PopulateShortLinksIfNone()
        {
            if (_shortLinks.Any())
            {
                return;
            }

            _logger.Warning("No short links in cache -- populating from repo");
            _shortLinks = _repo.GetShortLinks();
            _logger.Information("Afer populating from cache, there are now {numShortLinks} short links", _shortLinks.Count);
        }
        public List<ShortLinkItem> GetShortLinks()
        {
            PopulateShortLinksIfNone();
            return _shortLinks;
        }

        public ShortLinkItem GetByShortCode(string shortCode)
        {
            PopulateShortLinksIfNone();

            return _shortLinks
                .FirstOrDefault(x => x.ShortLinkCode.Equals(shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}