using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using xluhco.web.Controllers;
using xluhco.web.Repositories;
using Xunit;

namespace xluhco.web.tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<IShortLinkRepository> _mockRepo;
        private readonly Mock<ICacheEntry> _mockCacheEntry;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly HomeController _sut;

        public HomeControllerTests()
        {
            _mockRepo = new Mock<IShortLinkRepository>();
            _mockCacheEntry = new Mock<ICacheEntry>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(_mockCacheEntry.Object);

            _sut = new HomeController(_mockRepo.Object, _mockMemoryCache.Object);
        }

        [Fact]
        public void Ctor_NullRepository_ThrowsError()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HomeController(null, _mockMemoryCache.Object);

            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("repo");
        }

        [Fact]
        public void Index_ReturnsIndexView()
        {
            var result = _sut.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewName.Should().Be("Index");
        }

        [Fact]
        public async Task List_ReturnsListView()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(Task.FromResult(new List<ShortLinkItem>()));

            var result = await _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewName.Should().Be("List");
        }

        [Fact]
        public async Task List_NoItems_ReturnsEmptyList()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(Task.FromResult(new List<ShortLinkItem>()));

            var result = await _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ShortLinkItem>>(
                viewResult.ViewData.Model);

            model.Should().BeEmpty();
        }

        [Fact]
        public async Task List_OneListItem_ReturnsItem()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(Task.FromResult(new List<ShortLinkItem>() { new ShortLinkItem("abc", "http://seankilleen.com") }));

            var result = await _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ShortLinkItem>>(
                viewResult.ViewData.Model);

            // ReSharper disable PossibleMultipleEnumeration
            model.Should().HaveCount(1);
            model.First().ShortLinkCode.Should().Be("abc");
            // ReSharper enable PossibleMultipleEnumeration
        }

        [Fact]
        public async Task List_MultipleItems_ReturnsItemsSortedByShortCode()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(
                Task.FromResult(new List<ShortLinkItem>()
                {
                    new ShortLinkItem("ghi", "http://SeanKilleen.com"),
                    new ShortLinkItem("def", "http://SeanKilleen.com"),
                    new ShortLinkItem("abc", "http://SeanKilleen.com")
                }));

            var result = await _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ShortLinkItem>>(
                viewResult.ViewData.Model);

            model.Should().HaveCount(3);
            model.First().ShortLinkCode.Should().Be("abc");
            model.Last().ShortLinkCode.Should().Be("ghi");
        }
    }
}

