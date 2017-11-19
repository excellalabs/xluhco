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
                    null, 
                    Dummy.Of<ILogger>(),
                    Dummy.Of<IOptions<RedirectOptions>>(), 
                    Dummy.Of<IOptions<GoogleAnalyticsOptions>>());

                act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("shortLinkRepo");
            }
        }
    }

    public static class Dummy
    {
        public static T Of<T>() where T : class
        {
            return new Mock<T>().Object;
        }
    }

}
