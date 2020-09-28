using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Serilog;

namespace xluhco.web.Repositories
{
    public class BlobCsvConfiguration
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string FileName { get; set; }
    }
    public class BlobStorageCsvRepository : IShortLinkRepository
    {
        private BlobCsvConfiguration _configuration;
        private readonly ILogger _logger;
        public BlobStorageCsvRepository(IOptions<BlobCsvConfiguration> config, ILogger logger)
        {
            _configuration = config.Value ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<List<ShortLinkItem>> GetShortLinks()
        {
            _logger.Information("Beginning to populate short links");
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.ConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_configuration.ContainerName);
                var blobClient = containerClient.GetBlobClient(_configuration.FileName);

                using (var stream = await blobClient.OpenReadAsync())
                using (TextReader reader = new StreamReader(stream))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.CurrentCulture),
                    leaveOpen: false))
                {
                    _logger.Information("Reading shortLinks from blob");
                    csv.Configuration.MissingFieldFound = null;
                    var records = csv.GetRecords<ShortLinkItem>();

                    var shortLinks = records.ToList();
                    _logger.Information("Populated {numberOfShortLinks} short links", shortLinks.Count);

                    return shortLinks;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred when populating short links.");
                return new List<ShortLinkItem>();
            }
        }

        public async Task<ShortLinkItem> GetByShortCode(string shortCode)
        {
            var shortLinks = await GetShortLinks();

            return shortLinks
                .FirstOrDefault(x => x.ShortLinkCode.Equals(shortCode, StringComparison.InvariantCultureIgnoreCase));
        }
    }

    public class LocalCsvShortLinkRepository : IShortLinkRepository
    {
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public LocalCsvShortLinkRepository(ILogger logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public Task<List<ShortLinkItem>> GetShortLinks()
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