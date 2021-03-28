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


        private bool CheckBoardBoundaries(int width, int height)
        {
            return width >= 3 && height >= 3 && width <= 50 && height <= 50;
        }
    }
}