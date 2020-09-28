using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using xluhco.web.Controllers;
using xluhco.web.Repositories;
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

                act.Should().Throw<ArgumentNullException>()
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

                act.Should().Throw<ArgumentNullException>()
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

                act.Should().Throw<ArgumentNullException>()
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

                act.Should().Throw<ArgumentNullException>()
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

                act.Should().Throw<ArgumentNullException>()
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

                act.Should().Throw<ArgumentNullException>()
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

                act.Should().NotThrow();
            }
        }

        public class Index
        {
            private readonly RedirectController _sut;
            private readonly Mock<ILogger> _mockLogger = new Mock<ILogger>();
            private readonly Mock<IShortLinkRepository> _mockRepo = new Mock<IShortLinkRepository>();
            private readonly Mock<IOptions<GoogleAnalyticsOptions>> _mockGaOptions = new Mock<IOptions<GoogleAnalyticsOptions>>();
            private readonly Mock<IOptions<RedirectOptions>> _mockRedirectOptions = new Mock<IOptions<RedirectOptions>>();

            public Index()
            {
                _mockRedirectOptions.Setup(x => x.Value).Returns(new RedirectOptions());
                _mockGaOptions.Setup(x => x.Value).Returns(new GoogleAnalyticsOptions());

                _sut = new RedirectController(_mockRepo.Object, _mockLogger.Object, _mockRedirectOptions.Object, _mockGaOptions.Object);
            }

            [Fact]
            public async Task LogsTheRedirectShortCode()
            {
                await _sut.Index("abc");

                _mockLogger.Verify(x=> x.Debug("Entered the redirect for short code {shortCode}", "abc"));
            }

            [Fact]
            public async Task NoShortCode_LogsWarning()
            {
                _mockRepo.Setup(x => x.GetByShortCode(It.IsAny<string>())).Returns(Task.FromResult<ShortLinkItem>(null));

                await _sut.Index("abc");

                _mockLogger.Verify(x=>x.Warning("No redirect found for requested short code {shortCode}", "abc"));
            }

            [Fact]
            public async Task NoShortCode_ReturnsNotFound()
            {
                _mockRepo.Setup(x => x.GetByShortCode(It.IsAny<string>())).Returns(Task.FromResult<ShortLinkItem>(null));

                var result = await _sut.Index("abc");

                var viewResult = Assert.IsType<ViewResult>(result);

                viewResult.ViewName.Should().Be("NotFound");
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("    ")]
            public async Task EmptyShortCodeUrl_LogsWarning(string urlToTest)
            {
                _mockRepo.Setup(x => x.GetByShortCode(It.IsAny<string>())).Returns(Task.FromResult(new ShortLinkItem("abc", urlToTest)));

                await _sut.Index("abc");

                _mockLogger.Verify(x => x.Warning("No redirect found for requested short code {shortCode}", "abc"));
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("    ")]
            public async Task EmptyShortCodeUrl_ReturnsNotFound(string urlToTest)
            {
                _mockRepo.Setup(x => x.GetByShortCode(It.IsAny<string>())).Returns(Task.FromResult(new ShortLinkItem("abc", urlToTest)));

                var result = await _sut.Index("abc");

                var viewResult = Assert.IsType<ViewResult>(result);

                viewResult.ViewName.Should().Be("NotFound");
            }

            [Fact]
            public async Task UrlFound_LogsShortCodeAndURLAndTrackingId()
            {
                var testShortCode = "sk";
                var testUrl = "http://SeanKilleen.com";
                var testGaCode = "12345";

                _mockRepo.Setup(x => x.GetByShortCode(It.IsAny<string>()))
                    .Returns(Task.FromResult(new ShortLinkItem(testShortCode, testUrl)));

                _mockGaOptions.Setup(x => x.Value)
                    .Returns(new GoogleAnalyticsOptions {TrackingPropertyId = testGaCode});

                // need to recreate SUT instead of setup because .Value is used in the Ctor, 
                // meaning we can't just overwrite it with a Mock.
                var sut = new RedirectController(
                    _mockRepo.Object, 
                    _mockLogger.Object, 
                    _mockRedirectOptions.Object, 
                    _mockGaOptions.Object);

                await sut.Index("thisCodeDoesntMatter");

                _mockLogger.Verify(x=>x.Information("Redirecting {shortCode} to {redirectUrl} using tracking Id {gaTrackingId}", testShortCode, testUrl, testGaCode));
            }

            [Fact]
            public async Task UrlFound_ReturnsIndexPage()
            {
                _mockRepo.Setup(x => x.GetByShortCode(It.IsAny<string>()))
                    .Returns(Task.FromResult(new ShortLinkItem("test", "http://test.com")));

                var result = await _sut.Index("thisCodeDoesntMatter");

                var viewResult = Assert.IsType<ViewResult>(result);

                viewResult.ViewName.Should().Be("Index");
            }

            [Theory]
            [InlineData("sk", "http://SeanKilleen.com", "12345", 3, null, null, null)]
            [InlineData("sk", "http://SeanKilleen.com", "12345", 3, "http://images.com/test.jpg", "Test Title", "Test Description")]
            public async Task UrlFound_PopulatesViewModelCorrectly(string testShortCode, string testUrl, string testGaCode,
                int testSecondsToWait, string testImageUrl, string testTitle, string testDescription)
            {
                _mockRepo.Setup(x => x.GetByShortCode(It.IsAny<string>()))
                    .Returns(Task.FromResult(new ShortLinkItem(testShortCode, testUrl, testImageUrl, testTitle, testDescription)));

                _mockGaOptions.Setup(x => x.Value)
                    .Returns(new GoogleAnalyticsOptions { TrackingPropertyId = testGaCode });

                _mockRedirectOptions.Setup(x => x.Value)
                    .Returns(new RedirectOptions { SecondsToWaitForAnalytics = testSecondsToWait });

                // need to recreate SUT instead of setup because .Value is used in the Ctor, 
                // meaning we can't just overwrite it with a Mock.
                var sut = new RedirectController(
                    _mockRepo.Object,
                    _mockLogger.Object,
                    _mockRedirectOptions.Object,
                    _mockGaOptions.Object);

                var result = await sut.Index(testShortCode);

                var viewResult = Assert.IsType<ViewResult>(result);

                var model = Assert.IsAssignableFrom<RedirectViewModel>(
                    viewResult.ViewData.Model);

                model.ShortLinkCode.Should().Be(testShortCode);
                model.NumberOfSecondsToWait.Should().Be(testSecondsToWait);
                model.TrackingCode.Should().Be(testGaCode);
                model.ImageUrl.Should().Be(testImageUrl);
                model.Title.Should().Be(testTitle);
                model.Description.Should().Be(testDescription);
                model.Url.Should().Be(testUrl);
            }
        }
    }
}
