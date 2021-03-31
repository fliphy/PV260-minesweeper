using System;
using System.Collections.Generic;
using System.Net.Mime;
using NUnit.Framework;
using NUnit.Framework.Internal;


namespace MinesWeeper.Tests
{
    public class GameTests
    {
        private List<List<Item>> fakeBoard = new()
        {
            new List<Item>
            {
                new Item {HasMine = true},
                new Item {MinesArround = 2},
                new Item(),               
            },
            new List<Item>
            {
                new Item {HasMine = true},
                new Item {MinesArround = 2},
                new Item(),            
            },
            new List<Item>
            {
                new Item {HasMine = true},
                new Item {MinesArround = 2},
                new Item(),             
            },   
        };

        [Test]
        [TestCase(2,3)]
        [TestCase(3,2)]
        [TestCase(51, 50)]
        [TestCase(50, 51)]
        public void Game_StartGame_ThrowArgumentException(int width, int height)
        {
            var board = new Board();
            var game = new Game(board);
            
            Assert.Throws<ArgumentException>(() => game.StartGame(width, height));
        }
        
        [Test]
        [TestCase(10, 10)]
        [TestCase(25, 25)]
        [TestCase(50, 50)]
        public void Game_StartGame_CreateBoard(int width, int height)
        {
            var board = new Board();
            var game = new Game(board);
            game.StartGame(width, height);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(height, board.Height);
                Assert.AreEqual(width, board.Width);
            }); 
        }

        [Test]
        [TestCase(1, 1, -1)]
        [TestCase(1, 2, 0)]
        public void Game_PlayTurn_CorrectStateAfterTurn(int x, int y, int expected)
        {
            Board board = new Board();
            board.GameBoard = fakeBoard;
            board.Width = 3;
            board.Height = 3;
            
            var game = new Game(board);
            game.PlayTurn(x, y);
            
            Assert.AreEqual(expected, game.State);
        }
        
        [Test]
        public void Game_PlaceFlag_GameContinue()
        {
            var board = new Board();
            board.CreateBoard(3, 3);
            board.GameBoard = fakeBoard;
            board.Width = 3;
            board.Height = 3;
            
            var game = new Game(board);
            game.PlaceFlag(1, 1);
            
            Assert.AreEqual(0, game.State);
        }
        
        [Test]
        public void Game_PlaceFlag_GameWon()
        {
            var board = new Board();
            board.CreateBoard(3, 3);
            board.GameBoard = fakeBoard;
            board.Width = 3;
            board.Height = 3;
            
            var game = new Game(board);
            game.PlaceFlag(1, 1);
            game.PlaceFlag(2, 1);
            game.PlaceFlag(3, 1);
            
            Assert.AreEqual(1, game.State);
        }
    }
}