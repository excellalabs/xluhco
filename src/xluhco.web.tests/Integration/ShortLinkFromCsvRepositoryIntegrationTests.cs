using System;
using System.Linq;
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
        private readonly ShortLinkFromCsvRepository _sut;
        public ShortLinkFromCsvRepositoryIntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var hostingEnv = (IWebHostEnvironment)server.Host.Services.GetService(typeof(IWebHostEnvironment));
            _webRootPath = hostingEnv.WebRootPath;
            _sut = new ShortLinkFromCsvRepository(Dummy.Of<ILogger>(), hostingEnv);
        }

        [Fact]
        public void SettingHasWwwRoot()
        {
            _webRootPath.Should().NotBeEmpty();
            _webRootPath.Should().Contain("wwwroot");
        }

        [Fact]
        public void NonEmptyList()
        {
            var links = _sut.GetShortLinks();
            links.Should().NotBeEmpty();
        }

        [Fact]
        public void AllLinksAreValidUrls()
        {
            var urls = _sut.GetShortLinks().Select(x => x.URL).ToList();

            foreach (var url in urls)
            {
                // ReSharper disable once ObjectCreationAsStatement
                Action act = () => new Uri(url, UriKind.Absolute);
                act.Should().NotThrow();
            }
        }

        [Fact]
        public void NoDuplicateURLs()
        {
            var shortLinkCodes = _sut.GetShortLinks().Select(x => x.ShortLinkCode).ToList();

            var duplicateCodes = shortLinkCodes.GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            duplicateCodes.Should().BeEmpty();
        }
    }
}
