using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using xluhco.web.Controllers;
using Xunit;

namespace xluhco.web.tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly Mock<IShortLinkRepository> _mockRepo;
        private readonly HomeController _sut;

        public HomeControllerTests()
        {
            _mockRepo = new Mock<IShortLinkRepository>();
            _sut = new HomeController(_mockRepo.Object);
        }

        [Fact]
        public void Ctor_NullRepository_ThrowsError()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new HomeController(null);

            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("repo");
        }

        [Fact]
        public void Index_ReturnsIndexView()
        {
            var result = _sut.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewName.Should().Be("Index");
        }

        [Fact]
        public void List_ReturnsListView()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(new List<ShortLinkItem>());

            var result = _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewName.Should().Be("List");
        }

        [Fact]
        public void List_NoItems_ReturnsEmptyList()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(new List<ShortLinkItem>());

            var result = _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ShortLinkItem>>(
                viewResult.ViewData.Model);

            model.Should().BeEmpty();
        }

        [Fact]
        public void List_OneListItem_ReturnsItem()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(new List<ShortLinkItem>(){new ShortLinkItem("abc", "http://seankilleen.com")});

            var result = _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ShortLinkItem>>(
                viewResult.ViewData.Model);

            // ReSharper disable PossibleMultipleEnumeration
            model.Should().HaveCount(1);
            model.First().ShortLinkCode.Should().Be("abc");
            // ReSharper enable PossibleMultipleEnumeration
        }

        [Fact]
        public void List_MultipleItems_ReturnsItemsSortedByShortCode()
        {
            _mockRepo.Setup(x => x.GetShortLinks()).Returns(new List<ShortLinkItem>()
            {
                new ShortLinkItem("ghi", "http://seankilleen.com"),
                new ShortLinkItem("def", "http://seankilleen.com"),
                new ShortLinkItem("abc", "http://seankilleen.com")
            });

            var result = _sut.List();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ShortLinkItem>>(
                viewResult.ViewData.Model);

            model.Should().HaveCount(3);
            model.First().ShortLinkCode.Should().Be("abc");
            model.Last().ShortLinkCode.Should().Be("ghi");
        }
    }
}

