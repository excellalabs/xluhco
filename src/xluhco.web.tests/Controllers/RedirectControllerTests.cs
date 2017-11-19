using System;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using xluhco.web.Controllers;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

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

            [Fact]
            public void NullLogger_ThrowsError()
            {
                Action act = () => new RedirectController(
                    Dummy.Of<IShortLinkRepository>(),
                    null,
                    Dummy.Of<IOptions<RedirectOptions>>(),
                    Dummy.Of<IOptions<GoogleAnalyticsOptions>>());

                act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("logger");
            }

            [Fact]
            public void NullRedirectOptions_ThrowsError()
            {
                Action act = () => new RedirectController(
                    Dummy.Of<IShortLinkRepository>(),
                    Dummy.Of<ILogger>(),
                    null,
                    Dummy.Of<IOptions<GoogleAnalyticsOptions>>());

                act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("redirectOptions");
            }

            [Fact]
            public void NullRedirectOptionsValue_ThrowsError()
            {
                var mockRedirectOptions = new Mock<IOptions<RedirectOptions>>();
                mockRedirectOptions.Setup(x => x.Value).Returns((RedirectOptions) null);

                Action act = () => new RedirectController(
                    Dummy.Of<IShortLinkRepository>(),
                    Dummy.Of<ILogger>(),
                    mockRedirectOptions.Object,
                    null);

                act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("redirectOptions");
            }

            [Fact]
            public void NullAnalyticsOptions_ThrowsError()
            {
                var mockRedirectOptions = new Mock<IOptions<RedirectOptions>>();
                mockRedirectOptions.Setup(x => x.Value).Returns(new RedirectOptions());

                Action act = () => new RedirectController(
                    Dummy.Of<IShortLinkRepository>(),
                    Dummy.Of<ILogger>(),
                    mockRedirectOptions.Object,
                    null);

                act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("gaOptions");
            }

            [Fact]
            public void NullAnalyticsOptionsValue_ThrowsError()
            {
                var mockRedirectOptions = new Mock<IOptions<RedirectOptions>>();
                mockRedirectOptions.Setup(x => x.Value).Returns(new RedirectOptions());

                var mockAnalyticsOptions = new Mock<IOptions<GoogleAnalyticsOptions>>();
                mockAnalyticsOptions.Setup(x => x.Value).Returns((GoogleAnalyticsOptions) null);

                Action act = () => new RedirectController(
                    Dummy.Of<IShortLinkRepository>(),
                    Dummy.Of<ILogger>(),
                    mockRedirectOptions.Object,
                    mockAnalyticsOptions.Object);

                act.ShouldThrow<ArgumentNullException>()
                    .And.ParamName.Should().Be("gaOptions");
            }

            [Fact]
            public void AllValuesNonNull_DoesntThrowError()
            {
                var mockRedirectOptions = new Mock<IOptions<RedirectOptions>>();
                mockRedirectOptions.Setup(x => x.Value).Returns(new RedirectOptions());

                var mockAnalyticsOptions = new Mock<IOptions<GoogleAnalyticsOptions>>();
                mockAnalyticsOptions.Setup(x => x.Value).Returns(new GoogleAnalyticsOptions());

                Action act = () => new RedirectController(
                    Dummy.Of<IShortLinkRepository>(),
                    Dummy.Of<ILogger>(),
                    mockRedirectOptions.Object,
                    mockAnalyticsOptions.Object);

                act.ShouldNotThrow();
            }
        }
    }
}
