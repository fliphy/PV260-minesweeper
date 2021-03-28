using NUnit.Framework;
using FakeItEasy;

namespace MinesWeeper.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(10)]
        public void Test1(int pom)
        {
            Assert.AreEqual(pom, 9);
        }
    }
}