using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
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
    }
}

