using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace xluhco.web
{
    public class CachedShortLinkFromCsvRepository : IShortLinkRepository
    {
        private List<ShortLinkItem> _shortLinks;
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;

        public CachedShortLinkFromCsvRepository(IHostingEnvironment env, ILogger logger)
        {
            _env = env;
            _logger = logger;
            _shortLinks = new List<ShortLinkItem>();
        }

        private void PopulateShortLinks()
        {
            _logger.Information("Beginning to populate short links");

            var filePath = Path.Combine(_env.WebRootPath, "ShortLinks.csv");

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open))
                using (var reader = new StreamReader(fileStream))
                {
                    _logger.Information("Reading shortLinks from {filePath}", filePath);
                    var csv = new CsvReader(reader);
                    var records = csv.GetRecords<ShortLinkItem>();

                    _shortLinks = records.ToList();
                    _logger.Information("Populared {numberOfShortLinks} short links", _shortLinks.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while attempting to populate short linkes from {filePath}", filePath);
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