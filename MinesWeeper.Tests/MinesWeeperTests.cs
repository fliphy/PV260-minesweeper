using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using Microsoft.VisualBasic;

namespace MinesWeeper.Tests
{
    public class Tests
    {
        private List<List<Item>> fakeBoard = new()
        {
            new List<Item>
            {
                new Item {HasMine = true},
                new Item(),
                new Item(),
            },
            new List<Item>
            {
                new Item {HasMine = true},
                new Item(),
                new Item(),
            },
            new List<Item>
            {
                new Item {HasMine = true},
                new Item(),
                new Item(),
            }
        };


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(2, 3)]
        [TestCase(51, 50)]
        [TestCase(-1, 10)]
        [TestCase(2, 2)]
        public void Board_CreateBoard_InvalidArguments_ThrowsArgumentException(int width, int height)
        {
            Board gameBoard = new Board();
            Assert.Throws<ArgumentException>(() => gameBoard.CreateBoard(width, height));
        }

        [Test]
        [TestCase(5, 5)]
        [TestCase(3, 3)]
        [TestCase(50, 50)]
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


        [Test]
        [TestCase(5, 5)]
        [TestCase(14, 34)]
        [TestCase(3, 3)]
        [TestCase(50, 50)]
        public void Board_ItemsHaveCorrectMinesAroundValue(int width, int height)
        {
            var board = new Board();
            board.CreateBoard(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Assert.IsTrue(CheckFieldMinesNumber(i, j, board));
                }
            }
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(100, 100)]
        [TestCase(11, 1)]
        [TestCase(2, 11)]
        [TestCase(5, 0)]
        [TestCase(0, 5)]
        public void Board_PlayTurn_ThrowArgumentException(int x, int y)
        {
            Board gameBoard = new Board();
            gameBoard.CreateBoard(10, 10);
            Assert.Throws<ArgumentException>(() => gameBoard.PlayTurn(x, y));
        }

        [Test]
        public void Board_PlayTurn_FoundMineGameOver()
        {
            Board gameBoard = new Board();
            gameBoard.CreateBoard(3,3);
            gameBoard.GameBoard = fakeBoard;
            gameBoard.PlayTurn(1, 1);
            Assert.AreEqual(-1, gameBoard.State);
        }


        private bool CheckFieldMinesNumber(int x, int y, Board board)
        {
            HashSet<Tuple<int, int>> neighbours = new HashSet<Tuple<int, int>>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    if (x + i < board.Width && x + i >= 0 && y + j < board.Height && y + j >= 0)
                    {
                        neighbours.Add(new Tuple<int, int>(x + i, y + j));
                    }
                }
            }

            var itemMinesNeighbours = neighbours.Count(a => board.GameBoard[a.Item1][a.Item2].HasMine);
            return itemMinesNeighbours == board.GameBoard[x][y].MinesArround;
        }
    }
}