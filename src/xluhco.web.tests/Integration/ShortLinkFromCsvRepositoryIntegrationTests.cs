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
            var hostingEnv = (IHostingEnvironment)server.Host.Services.GetService(typeof(IHostingEnvironment));
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
                act.ShouldNotThrow();
            }
        }
    }
}
