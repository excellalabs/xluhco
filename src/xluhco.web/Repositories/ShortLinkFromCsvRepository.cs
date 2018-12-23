using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace xluhco.web.Repositories
{
    public class ShortLinkFromCsvRepository : IShortLinkRepository
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        public ShortLinkFromCsvRepository(ILogger logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public List<ShortLinkItem> GetShortLinks()
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
                    csv.Configuration.MissingFieldFound = null;
                    var records = csv.GetRecords<ShortLinkItem>();

                    var shortLinks = records.ToList();
                    _logger.Information("Populated {numberOfShortLinks} short links", shortLinks.Count);

                    return shortLinks;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while attempting to populate short linkes from {filePath}", filePath);
                return new List<ShortLinkItem>();
            }
        }

        public ShortLinkItem GetByShortCode(string shortCode)
        {
            var shortLinks = GetShortLinks();

            return shortLinks
                .FirstOrDefault(x => x.ShortLinkCode.Equals(shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}