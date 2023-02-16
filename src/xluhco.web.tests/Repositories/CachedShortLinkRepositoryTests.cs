using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Serilog;
using xluhco.web.Repositories;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

namespace xluhco.web.tests.Repositories
{
    public class CachedShortLinkRepositoryTests
    {
        public class Ctor
        {
            [Fact]
            public void NullLogger_ThrowsException()
            {
                Action act = () => new CachedShortLinkRepository(null, Dummy.Of<IShortLinkRepository>());

                act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("logger");
            }

            [Fact]
            public void NullRepo_ThrowsException()
            {
                Action act = () => new CachedShortLinkRepository(Dummy.Of<ILogger>(), null);

                act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("repo");
            }

            [Fact]
            public void AllDependencies_DoesntThrow()
            {
                Action act = () => new CachedShortLinkRepository(
                    Dummy.Of<ILogger>(), 
                    Dummy.Of<IShortLinkRepository>());

                act.Should().NotThrow();
            }
        }

        public class GetShortLinks
        {
            private readonly CachedShortLinkRepository _sut;
            private readonly Mock<IShortLinkRepository> _mockRepo = new Mock<IShortLinkRepository>();
            private readonly Mock<ILogger> _mockLogger = new Mock<ILogger>();

            public GetShortLinks()
            {
                _sut = new CachedShortLinkRepository(
                    _mockLogger.Object, 
                    _mockRepo.Object);

                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>()));
            }

            [Fact]
            public async Task IfLinksInCache_ReturnsLinks()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>
                    {
                        new ShortLinkItem("abc", "blahblah"),
                        new ShortLinkItem("def", "blahblah")
                    }));

                var result= await _sut.GetShortLinks();

                result.Should().HaveCount(2);

                result.First().ShortLinkCode.Should().Be("abc");
                result.Last().ShortLinkCode.Should().Be("def");
            }

            [Fact]
            public async Task IfNoLinks_LogsWarning()
            {
                await _sut.GetShortLinks();

                _mockLogger.Verify(x=>x.Warning("No short links in cache -- populating from repo"), Times.Once);
            }

            [Fact]
            public async Task IfNoLinks_PopulatesFromRepo()
            {
                await _sut.GetShortLinks();

                _mockRepo.Verify(x=>x.GetShortLinks(), Times.Once);
            }

            [Fact]
            public async Task IfNoLinks_AfterPopulating_LogsCount()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>
                    {
                        new ShortLinkItem("abc", "blahblah"),
                        new ShortLinkItem("def", "blahblah")
                    }));

                await _sut.GetShortLinks();

                _mockLogger.Verify(x=>x.Information("After populating from cache, there are now {numShortLinks} short links", 2));
            }

            [Fact]
            public async Task IfPopulatedWithNoLinks_CallsRepoAgain()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>()));

                await _sut.GetShortLinks();
                await _sut.GetShortLinks();

                _mockRepo.Verify(x=>x.GetShortLinks(), Times.Exactly(2));
            }

            [Fact]
            public async Task IfPopulatedWithNoLinks_ReturnsEmptyList()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>()));

                var result = await _sut.GetShortLinks();

                result.Should().NotBeNull();
                result.Should().BeEmpty();
            }
        }

        public class GetByShortCode
        {
            private readonly CachedShortLinkRepository _sut;
            private readonly Mock<IShortLinkRepository> _mockRepo = new Mock<IShortLinkRepository>();
            private readonly Mock<ILogger> _mockLogger = new Mock<ILogger>();

            public GetByShortCode()
            {
                _sut = new CachedShortLinkRepository(
                    _mockLogger.Object,
                    _mockRepo.Object);

                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>()));
            }


            [Fact]
            public async Task IfLinksInCache_ReturnsMatchingLink()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>
                    {
                        new ShortLinkItem("abc", "blahblahabc"),
                        new ShortLinkItem("def", "blahblah")
                    }));

                var result = await _sut.GetByShortCode("abc");

                result.URL.Should().Be("blahblahabc");
            }

            [Fact]
            public async Task IfLinksInCacheButNoneMatching_ReturnNull()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>
                    {
                        new ShortLinkItem("abc", "blahblahabc"),
                        new ShortLinkItem("def", "blahblah")
                    }));

                var result = await _sut.GetByShortCode("wontbefound");

                result.Should().BeNull();
            }

            [Fact]
            public async Task IfNoLinks_LogsWarning()
            {
                await _sut.GetByShortCode("test");

                _mockLogger.Verify(x => x.Warning("No short links in cache -- populating from repo"), Times.Once);
            }

            [Fact]
            public async Task IfNoLinks_PopulatesFromRepo()
            {
                await _sut.GetByShortCode("test");

                _mockRepo.Verify(x=>x.GetShortLinks(), Times.Once);
            }

            [Fact]
            public async Task IfNoLinks_AfterPopulating_LogsCount()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>
                    {
                        new ShortLinkItem("abc", "blahblah"),
                        new ShortLinkItem("def", "blahblah")
                    }));

                await _sut.GetByShortCode("test");

                _mockLogger.Verify(x => x.Information("After populating from cache, there are now {numShortLinks} short links", 2));
            }

            [Fact]
            public async Task IfPopulatedWithNoLinks_CallsRepoAgain()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>()));

                await _sut.GetByShortCode("test");
                await _sut.GetByShortCode("test");

                _mockRepo.Verify(x => x.GetShortLinks(), Times.Exactly(2));
            }

            [Fact]
            public async Task IfPopulatedWithNoLinks_ReturnsNull()
            {
                _mockRepo.Setup(x => x.GetShortLinks())
                    .Returns(Task.FromResult(new List<ShortLinkItem>()));

                var result = await _sut.GetByShortCode("test");

                result.Should().BeNull();
            }
        }
    }
}
