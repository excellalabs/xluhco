using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using xluhco.web.Controllers;
using Xunit;

namespace xluhco.web.tests.Controllers
{
    public class HomeControllerTests
    {
        private Mock<IShortLinkRepository> _mockRepo;
        private HomeController _sut;

        public HomeControllerTests()
        {
            _mockRepo = new Mock<IShortLinkRepository>();
            _sut = new HomeController(_mockRepo.Object);
        }

        [Fact]
        public void Ctor_NullRepository_ThrowsError()
        {
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
    }
}

