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
        
        [Test]
        [TestCase(10, 10)]
        [TestCase(50, 50)]
        [TestCase(3, 3)]
        [TestCase(3, 4)]
        [TestCase(25, 44)]
        public void Board_CreateBoard_ValidAmountOfMines(int width, int height)
        {
            var board = new Board();
            board.CreateBoard(width, height);
            var itemCount = width * height;
            var minCount = Convert.ToInt32(Math.Ceiling(0.2 * itemCount));
            var maxCount = Convert.ToInt32(Math.Floor(0.6 * itemCount));
            
            var mineCount = board.GameBoard.Sum(row => row.Count(item => item.HasMine));

            Assert.Multiple(() =>
            {
                Assert.That(mineCount >= minCount);
                Assert.That(mineCount <= maxCount);
            });
        }
        
    }
}