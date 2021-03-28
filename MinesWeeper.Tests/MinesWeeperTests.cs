using System;
using System.Linq;
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
            Assert.Throws<ArgumentException>(() => gameBoard.CreateBoard(width, height));
        }

        [Test]
        [TestCase(5,5)]
        [TestCase(3,3)]
        [TestCase(50,50)]
        public void Board_CreateBoard_ValidArguments(int width, int height)
        {
            Board b = new Board();
            b.CreateBoard(width, height);
            Assert.Multiple(() =>
            {
                Assert.That(() => b.GameBoard.Count == width);
                Assert.That(() => b.GameBoard.First().Count == height);
            });
        }
    }
}