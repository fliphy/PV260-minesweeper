using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MinesWeeper.Tests
{
    public class Tests
    {
        private Board board;

        [SetUp]
        public void Setup()
        {
            board = new Board();
            board.Width = 4;
            board.Height = 5;
            board.MinesCoords = new HashSet<Tuple<int, int>>()
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(2, 4),
                new(3, 0),
            };
            board.GameBoard = new()
            {
                new List<Item>
                {
                    new Item {HasMine = true},
                    new Item {MinesArround = 2},
                    new Item(),
                    new Item(),
                    new Item(),
                },
                new List<Item>
                {
                    new Item {HasMine = true},
                    new Item {MinesArround = 3},
                    new Item(),
                    new Item {MinesArround = 1},
                    new Item {MinesArround = 1},
                },
                new List<Item>
                {
                    new Item {HasMine = true},
                    new Item {MinesArround = 3},
                    new Item(),
                    new Item {MinesArround = 1},
                    new Item {HasMine = true},
                },
                new List<Item>
                {
                    new Item {HasMine = true},
                    new Item {MinesArround = 2},
                    new Item(),
                    new Item {MinesArround = 1},
                    new Item {MinesArround = 1},
                }
            };
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
            var gameBoard = new Board();
            gameBoard.CreateBoard(width, height);
            var itemCount = width * height;
            var minCount = Convert.ToInt32(Math.Ceiling(0.2 * itemCount));
            var maxCount = Convert.ToInt32(Math.Floor(0.6 * itemCount));

            var mineCount = gameBoard.GameBoard.Sum(row => row.Count(item => item.HasMine));

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
            var gameBoard = new Board();
            gameBoard.CreateBoard(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Assert.IsTrue(CheckFieldMinesNumber(i, j, gameBoard));
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
        public void Board_PlayTurn_RevealsCorrectArea()
        {
            board.PlayTurn(1, 4);

            var correctRevealed = new List<Tuple<int, int>>()
            {
                new(1, 2),
                new(1, 3),
                new(1, 4),
                new(1, 5),
                new(2, 2),
                new(2, 3),
                new(2, 4),
                new(2, 5),
                new(3, 2),
                new(3, 3),
                new(3, 4),
                new(4, 2),
                new(4, 3),
                new(4, 4),
            };
            foreach (var (x, y) in correctRevealed)
            {
                var tmpX = x - 1;
                var tmpY = y - 1;
                Assert.True(board.GameBoard[tmpX][tmpY].Revealed);
            }
        }

        [Test]
        [TestCase(1, 2)]
        public void Board_PlayTurn_ClickOnNumber_RevealsNumberOnly(int x, int y)
        {
            board.PlayTurn(x, y);
            Assert.AreEqual(1, board.GameBoard.Sum(row => row.Count(item => item.Revealed)));
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(11, 3)]
        [TestCase(-1, 4)]
        [TestCase(11, 11)]
        public void Board_PlaceFlag_ThrowsArgumentException(int x, int y)
        {
            var gameBoard = new Board();
            gameBoard.CreateBoard(10, 10);
            Assert.Throws<ArgumentException>(() => gameBoard.PlaceFlag(x, y));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 5)]
        [TestCase(10, 10)]
        [TestCase(9, 4)]
        public void Board_PlaceFlag_ItemHasFlag(int x, int y)
        {
            var gameBoard = new Board();
            gameBoard.CreateBoard(10, 10);
            gameBoard.PlaceFlag(x, y);
            Assert.True(gameBoard.GetItem(x, y).HasFlag);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 5)]
        [TestCase(10, 10)]
        [TestCase(9, 4)]
        public void Board_PlaceFlag_ItemDoesntHaveFlag(int x, int y)
        {
            var gameBoard = new Board();
            gameBoard.CreateBoard(10, 10);
            gameBoard.PlaceFlag(x, y);
            gameBoard.PlaceFlag(x, y);
            Assert.False(gameBoard.GetItem(x, y).HasFlag);
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