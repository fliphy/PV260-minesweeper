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
        [TestCase(1, 1, GameState.GameOver)]
        [TestCase(1, 2, GameState.GameOn)]
        public void Game_PlayTurn_CorrectStateAfterTurn(int x, int y, GameState expected)
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
            
            Assert.AreEqual(GameState.GameOn, game.State);
        }
        
        [Test]
        public void Game_PlaceFlag_GameWon()
        {
            var board = new Board();
            //board.CreateBoard(3, 3);
            board.GameBoard = fakeBoard;
            board.Width = 3;
            board.Height = 3;
            board.MineCount = 3;
            board.MinesCoords = new HashSet<Tuple<int, int>>()
            {
                new(0, 0),
                new(1, 0),
                new(2, 0)
            };
            
            var game = new Game(board);
            game.PlaceFlag(1, 1);
            game.PlaceFlag(2, 1);
            game.PlaceFlag(3, 1);
            
            Assert.That(() => game.State.Equals(GameState.GameWon));
        }
    }
}