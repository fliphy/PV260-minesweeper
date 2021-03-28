using System;

namespace MinesWeeper
{
    public class Board
    {
        public Item[,] GameBoard { get; private set; }

        public Board()
        {

        }

        public void CreateBoard(int width, int height)
        {
            if (!CheckBoardBoundaries(width, height))
            {
                throw new ArgumentException("Invalid board boundaries");
            }

            GameBoard = new Item[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GameBoard[i, j] = new Item();
                }
            }
        }


        private bool CheckBoardBoundaries(int width, int height)
        {
            return width >= 3 && height >= 3 && width <= 50 && height <= 50;
        }
    }
}