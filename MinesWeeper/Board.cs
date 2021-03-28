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

            SetUpMines(width, height);
        }

        private void SetUpMines(int width, int height)
        {
            var random = new Random();
            var itemCount = width * height;
            var minCount = Convert.ToInt32(Math.Ceiling(0.2 * itemCount));
            var maxCount = Convert.ToInt32(Math.Floor(0.6 * itemCount));
            var mineCount = random.Next(minCount, maxCount + 1);

            var mineCoords = GenerateMineCords(mineCount, width, height);
            PlaceMines(mineCoords);      
        }

        private HashSet<Tuple<int, int>> GenerateMineCords(int mineCount, int width, int height)
        {

            var random = new Random();
            var mineCoords = new HashSet<Tuple<int, int>>();
            while (mineCoords.Count != mineCount)
            {
                var x = random.Next(width);
                var y = random.Next(height);
                mineCoords.Add(new Tuple<int, int>(x, y));
            }

            return mineCoords;
        }

        private void PlaceMines(HashSet<Tuple<int, int>> mineCoords)
        {
            foreach (var (x, y) in mineCoords)
            {
                GameBoard[x][y].HasMine = true;
            }
        }




        private bool CheckBoardBoundaries(int width, int height)
        {
            return width >= 3 && height >= 3 && width <= 50 && height <= 50;
        }
    }
}