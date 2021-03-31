using System;
using System.Collections.Generic;
using NUnit.Framework;


namespace MinesWeeper.Tests
{
    public class GameTests
    {
        private Board board;

        [SetUp]
        public void SetUp()
        {
            board = new Board();
            board.CreateBoard(3, 3);
            board.GameBoard = new()
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
            };;
            board.Width = 3;
            board.Height = 3;
            board.MineCount = 3;
            board.MinesCoords = new HashSet<Tuple<int, int>>()
            {
                new(0, 0),
                new(1, 0),
                new(2, 0)
            };
        }

        [Test]
        [TestCase(2,3)]
        [TestCase(3,2)]
        [TestCase(51, 50)]
        [TestCase(50, 51)]
        public void Game_StartGame_ThrowArgumentException(int width, int height)
        {
            var gameBoard = new Board();
            var game = new Game(gameBoard);
            
            Assert.Throws<ArgumentException>(() => game.StartGame(width, height));
        }
        
        [Test]
        [TestCase(10, 10)]
        [TestCase(25, 25)]
        [TestCase(50, 50)]
        public void Game_StartGame_CreateBoard(int width, int height)
        {
            var gameBoard = new Board();
            var game = new Game(gameBoard);
            game.StartGame(width, height);
            
            Assert.Multiple(() =>
            {
                Assert.That(game.State.Equals(GameState.GameOn));
                Assert.AreEqual(height, gameBoard.Height);
                Assert.AreEqual(width, gameBoard.Width);
            }); 
        }

        [Test]
        [TestCase(1, 1, GameState.GameOver)]
        [TestCase(1, 2, GameState.GameOn)]
        public void Game_PlayTurn_CorrectStateAfterTurn(int x, int y, GameState expected)
        {
            var game = new Game(board);
            game.PlayTurn(x, y);
            
            Assert.AreEqual(expected, game.State);
        }
        
        [Test]
        public void Game_PlaceFlag_GameContinue()
        {
            var game = new Game(board);
            game.PlaceFlag(1, 1);
            
            Assert.AreEqual(GameState.GameOn, game.State);
        }
        
        [Test]
        public void Game_PlaceFlag_GameWon()
        {
            var game = new Game(board);
            game.PlaceFlag(1, 1);
            game.PlaceFlag(2, 1);
            game.PlaceFlag(3, 1);
            
            Assert.That(() => game.State.Equals(GameState.GameWon));
        }
    }
}