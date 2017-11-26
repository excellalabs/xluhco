using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Serilog;
using Xunit;

namespace xluhco.web.tests.Integration
{
    public class ShortLinkFromCsvRepositoryIntegrationTests
    {
        private readonly string _webRootPath;
        public ShortLinkFromCsvRepositoryIntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            var hostingEnv = (IHostingEnvironment)server.Host.Services.GetService(typeof(IHostingEnvironment));
            this._webRootPath = hostingEnv.WebRootPath;
        }

        [Fact]
        public void SettingHasWwwRoot()
        {
            _webRootPath.Should().NotBeEmpty();
            _webRootPath.Should().Contain("wwwroot");
        }
    }
}
