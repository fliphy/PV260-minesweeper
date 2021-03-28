using System;
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
        [TestCase(2,3)]
        [TestCase(51,50)]
        [TestCase(-1, 10)]
        [TestCase(2,2)]
        public void Board_CreateBoard_InvalidArguments_ThrowsArgumentException(int width, int height)
        {
            Board gameBoard = new Board();
            Assert.Throws<ArgumentException>(gameBoard.createBoard(width, height));
        }
    }
}