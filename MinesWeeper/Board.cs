using System;
using System.Collections.Generic;

namespace MinesWeeper
{
    public class Board
    {
        public List<List<Item>> GameBoard { get; private set; }

        public Board()
        {

        }

        public void CreateBoard(int width, int height)
        {
            if (!CheckBoardBoundaries(width, height))
            {
                throw new ArgumentException("Invalid board boundaries");
            }
            InitializeBoard(width, height);
            
        }


        private void InitializeBoard(int width, int height)
        {
            GameBoard = new List<List<Item>>();
            for (int i = 0; i < width; i++)
            {
                GameBoard.Add(new List<Item>());
                for (int j = 0; j < height; j++)
                {
                    GameBoard[i].Add(new Item());
                }
            }
        }

        private void SetUpMines(int width, int height)
        {
            var itemCount = width * height;
            var minCount = Convert.ToInt32(Math.Ceiling(0.2 * itemCount));
            var maxCount = Convert.ToInt32(Math.Floor(0.6 * itemCount));

            var random = new Random();
            var mineCount = random.Next(minCount, maxCount);
            
        }


        private bool CheckBoardBoundaries(int width, int height)
        {
            return width >= 3 && height >= 3 && width <= 50 && height <= 50;
        }
    }
}