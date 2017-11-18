using System;
using FluentAssertions;
using Xunit;

namespace xluhco.web.tests
{
    public class UnitTest1
    {
        [Fact]
        public void MathWorks()
        {
            var sum = 1 + 1;
            sum.Should().Be(2);
        }
    }
}
