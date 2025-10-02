using NUnit.Framework;

namespace TravelTayoUnitTest
{
    [TestFixture]           // Required on class
    public class SampleTest
    {
        [Test]              // Required on method
        public void AssertIsNotNullWorks()
        {
            object obj = new object();
            Assert.IsNotNull(obj);
        }
    }
}
