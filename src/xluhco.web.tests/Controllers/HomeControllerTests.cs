using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using xluhco.web.Controllers;
using Xunit;

namespace xluhco.web.tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Ctor_NullRepository_ThrowsError()
        {
            Action act = () => new HomeController(null);

            act.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("repo");
        }

        [Fact]
        public void Index_ReturnsIndexView()
        {
            var sut = new HomeController(new Mock<IShortLinkRepository>().Object);

            var result = sut.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewName.Should().Be("Index");
        }
    }
}

