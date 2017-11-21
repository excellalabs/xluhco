using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace xluhco.web.tests.Repositories
{
    public class CachedShortLinkRepositoryTests
    {
        public class Ctor
        {
            [Fact]
            public void NullLogger_ThrowsException()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void NullRepo_ThrowsException()
            {
                throw new NotImplementedException();
            }
        }

        public class GetShortLinks
        {
            [Fact]
            public void IfLinksInCache_ReturnsLinks()
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
