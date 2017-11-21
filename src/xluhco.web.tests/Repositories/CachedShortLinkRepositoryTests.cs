using System;
using System.Collections.Generic;
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

                act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("logger");
            }

            [Fact]
            public void NullRepo_ThrowsException()
            {
                Action act = () => new CachedShortLinkRepository(Dummy.Of<ILogger>(), null);

                act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("repo");
            }

            [Fact]
            public void AllDependencies_DoesntThrow()
            {
                Action act = () => new CachedShortLinkRepository(
                    Dummy.Of<ILogger>(), 
                    Dummy.Of<IShortLinkRepository>());

                act.ShouldNotThrow();
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
                    .Returns(new List<ShortLinkItem>());
            }

            [Fact]
            public void IfLinksInCache_ReturnsLinks()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void IfNoLinks_LogsWarning()
            {
                _sut.GetShortLinks();

                _mockLogger.Verify(x=>x.Warning("No short links in cache -- populating from repo"), Times.Once);
            }

            [Fact]
            public void IfNoLinks_PopulatesFromRepo()
            {
                throw new NotImplementedException();

            }

            [Fact]
            public void IfNoLinks_AfterPopulating_LogsCount()
            {
                throw new NotImplementedException();

            }

            [Fact]
            public void IfPopulatedWithNoLinks_CallsRepoAgain()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void IfPopulatedWithNoLinks_ReturnsEmptyList()
            {
                throw new NotImplementedException();

            }

        }

        public class GetByShortCode
        {
            [Fact]
            public void IfLinksInCache_ReturnsMatchingLink()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void IfLinksInCacheButNoneMatching_ReturnNull()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void IfNoLinks_LogsWarning()
            {
                throw new NotImplementedException();

            }

            [Fact]
            public void IfNoLinks_PopulatesFromRepo()
            {
                throw new NotImplementedException();

            }

            [Fact]
            public void IfNoLinks_AfterPopulating_LogsCount()
            {
                throw new NotImplementedException();

            }

            [Fact]
            public void IfPopulatedWithNoLinks_CallsRepoAgain()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void IfPopulatedWithNoLinks_ReturnsEmptyList()
            {
                throw new NotImplementedException();

            }
        }
    }
}
