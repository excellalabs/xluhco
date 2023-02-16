using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly CsvConfiguration _config;

        public LocalCsvShortLinkRepository(ILogger logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;

            _config = new CsvConfiguration(CultureInfo.CurrentCulture);
            _config.MissingFieldFound = null;

        }

        public Task<List<ShortLinkItem>> GetShortLinks()
        {
            _logger.Information("Beginning to populate short links");

            var filePath = Path.Combine(_env.WebRootPath, "ShortLinks.csv");

            try
            {
                using (TextReader reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, _config,
                    leaveOpen: false))
                {
                    _logger.Information("Reading shortLinks from {filePath}", filePath);
                    var records = csv.GetRecords<ShortLinkItem>();

                    var shortLinks = records.ToList();
                    _logger.Information("Populated {numberOfShortLinks} short links", shortLinks.Count);

                    return Task.FromResult(shortLinks);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while attempting to populate short linkes from {filePath}", filePath);
                return Task.FromResult(new List<ShortLinkItem>());
            }
        }

        public async Task<ShortLinkItem> GetByShortCode(string shortCode)
        {
            var shortLinks = await GetShortLinks();

            return shortLinks
                .FirstOrDefault(x => x.ShortLinkCode.Equals(shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}