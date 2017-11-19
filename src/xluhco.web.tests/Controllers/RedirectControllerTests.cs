using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using xluhco.web.Controllers;
using Xunit;

namespace xluhco.web.tests.Controllers
{
    public class RedirectControllerTests
    {
        public class Ctor
        {
            [Fact]
            public void NullShortLinkRepository_ThrowsError()
            {
                Action act = () => new RedirectController(
                    null, new Mock<ILogger>().Object,
                    new Mock<IOptions<RedirectOptions>>().Object, 
                    new Mock<IOptions<GoogleAnalyticsOptions>>().Object);

                act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("shortLinkRepo");
            }
        }

    }
}
