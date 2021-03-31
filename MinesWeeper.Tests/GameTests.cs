using System.Collections.Generic;
using NUnit.Framework;


namespace MinesWeeper.Tests
{
    public class GameTests
    {
        
        [Test]
        [TestCase(10, 10)]
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