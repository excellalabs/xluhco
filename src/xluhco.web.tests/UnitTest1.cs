using System;
using Xunit;

namespace xluhco.web.tests
{
    public class UnitTest1
    {
        [Fact]
        public void MathWorks()
        {
            var sum = 1 + 1;

            Assert.Equal(2, sum);
        }
    }
}
