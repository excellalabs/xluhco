using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace xluhco.web.Repositories
{
    public class LocalCsvShortLinkRepository : IShortLinkRepository
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public LocalCsvShortLinkRepository(ILogger logger, IWebHostEnvironment env)
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
                using (TextReader reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture),
                    leaveOpen: false))
                {
                    _logger.Information("Reading shortLinks from {filePath}", filePath);
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