using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

namespace xluhco.web.Repositories
{
    public class BlobStorageCsvRepository : IShortLinkRepository
    {
        private readonly BlobCsvConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly CsvConfiguration _config;
        public BlobStorageCsvRepository(IOptions<BlobCsvConfiguration> config, ILogger logger)
        {
            _configuration = config.Value ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                MissingFieldFound = null
            };
        }
        public async Task<List<ShortLinkItem>> GetShortLinks()
        {
            _logger.Information("Beginning to populate short links");
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.ConnectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(_configuration.ContainerName);
                var blobClient = containerClient.GetBlobClient(_configuration.FileName);
                
                await using var stream = await blobClient.OpenReadAsync();
                using TextReader reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, _config, leaveOpen: false);
                
                _logger.Information("Reading shortLinks from blob");
                var records = csv.GetRecords<ShortLinkItem>();

                var shortLinks = records.ToList();
                _logger.Information("Populated {numberOfShortLinks} short links", shortLinks.Count);

                return shortLinks;
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
}