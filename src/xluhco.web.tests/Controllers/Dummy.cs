using Moq;

namespace xluhco.web.tests.Controllers
{
    public static class Dummy
    {
        public static T Of<T>() where T : class
        {
            return new Mock<T>().Object;
        }

    }
}