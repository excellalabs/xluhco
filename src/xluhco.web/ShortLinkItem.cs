using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;

namespace xluhco.web
{
    public class ShortLinkItem : IShortLinkItem
    {
        public string ShortLinkCode { get; private set; }
        public string URL { get; private set; }

        public ShortLinkItem(string shortLinkCode, string url)
        {
            ShortLinkCode = shortLinkCode;
            URL = url;
        }
    }

    public interface IShortLinkRepository
    {
        List<ShortLinkItem> GetShortLinks();
        ShortLinkItem GetByShortCode(string shortCode);
    }

    public class CachedShortLinkFromCsvRepository : IShortLinkRepository
    {
        private List<ShortLinkItem> _shortLinks;
        private readonly IHostingEnvironment _env;

        public CachedShortLinkFromCsvRepository(IHostingEnvironment env)
        {
            _env = env;
            _shortLinks = new List<ShortLinkItem>();
        }

        private void PopulateShortLinks()
        {
            using (var fileStream = new FileStream(Path.Combine(_env.WebRootPath, "ShortLinks.csv"), FileMode.Open))
            using (var reader = new StreamReader(fileStream))
            {
                var csv = new CsvReader(reader);
                var records = csv.GetRecords<ShortLinkItem>();

                _shortLinks = records.ToList();
            }
        }

        public List<ShortLinkItem> GetShortLinks()
        {
            if (!_shortLinks.Any())
            {
                PopulateShortLinks();
            }

            return _shortLinks;
        }

        public ShortLinkItem GetByShortCode(string shortCode)
        {
            if (!_shortLinks.Any())
            {
                PopulateShortLinks();
            }
            return _shortLinks
                .FirstOrDefault(x => x.ShortLinkCode.Equals(shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
