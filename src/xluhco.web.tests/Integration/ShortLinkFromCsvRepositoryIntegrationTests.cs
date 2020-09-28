using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Serilog;
using xluhco.web.Repositories;
using Xunit;

namespace xluhco.web.tests.Integration
{
    public class ShortLinkFromCsvRepositoryIntegrationTests
    {
        private readonly string _webRootPath;
        private readonly LocalCsvShortLinkRepository _sut;
        public ShortLinkFromCsvRepositoryIntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var hostingEnv = (IWebHostEnvironment)server.Host.Services.GetService(typeof(IWebHostEnvironment));
            _webRootPath = hostingEnv.WebRootPath;
            _sut = new LocalCsvShortLinkRepository(Dummy.Of<ILogger>(), hostingEnv);
        }

        [Fact]
        public void SettingHasWwwRoot()
        {
            _webRootPath.Should().NotBeEmpty();
            _webRootPath.Should().Contain("wwwroot");
        }

        [Fact]
        public async Task NonEmptyList()
        {
            var links = await _sut.GetShortLinks();
            links.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AllLinksAreValidUrls()
        {
            var links = await _sut.GetShortLinks();
            var urls = links.Select(x => x.URL).ToList();

            foreach (var url in urls)
            {
                // ReSharper disable once ObjectCreationAsStatement
                Action act = () => new Uri(url, UriKind.Absolute);
                act.Should().NotThrow();
            }
        }

        [Fact]
        public async Task NoDuplicateURLs()
        {
            var links = await _sut.GetShortLinks();
            var shortLinkCodes = links.Select(x => x.ShortLinkCode).ToList();

            var duplicateCodes = shortLinkCodes.GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            duplicateCodes.Should().BeEmpty();
        }
    }
}
