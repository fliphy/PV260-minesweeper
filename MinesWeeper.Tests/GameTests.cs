using System;
using System.Collections.Generic;
using System.Net.Mime;
using NUnit.Framework;
using NUnit.Framework.Internal;


namespace MinesWeeper.Tests
{
    public class GameTests
    {

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
        public void CreateBoard(int width, int height)
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
    }
}