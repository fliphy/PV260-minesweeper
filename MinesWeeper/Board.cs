using System;
using System.Collections.Generic;
using System.Linq;

namespace MinesWeeper
{
    public class Board: IBoard
    {
        public List<List<Item>> GameBoard { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int State { get; set; }

        public Board()
        {

        }

        public void CreateBoard(int width, int height)
        {
            Width = width;
            Height = height;
            if (!CheckBoardInitBoundaries(width, height))
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
                var neighbors = GetItemNeighbors(x, y);
                foreach (var (a, b) in neighbors)
                {
                    GameBoard[a][b].MinesArround++;
                }
            }
        }
        
        public void PlayTurn(int x, int y)
        {
            DecrementPosition(ref x, ref y);
            if(!CheckBoardBoundaries(x, y))
            {
                throw new ArgumentException("Cords out of bounds");
            }

            State = (GameBoard[x][y].HasMine) ? -1 : 0;
            GameBoard[x][y].Revealed = true;
            if (GameBoard[x][y].MinesArround == 0)
            {
                RevealAvailableArea(x, y);
            }
        }

        private void RevealAvailableArea(int x, int y)
        {
            var queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(new Tuple<int, int >(x, y));
            
            while (queue.Any())
            {
                var (currentX, currentY) = queue.Dequeue();
                GameBoard[currentX][currentY].Revealed = true;
                var itemNeighbours = GetItemNeighbors(currentX, currentY);
                if (GameBoard[currentX][currentY].MinesArround > 0) continue;
                foreach (var item in itemNeighbours
                    .Where(item => !GameBoard[item.Item1][item.Item2].Revealed))
                {
                    queue.Enqueue(item);
                }
            }
        }

        public void PlaceFlag(int x, int y)
        {
            DecrementPosition(ref x, ref y);
            if (!CheckBoardBoundaries(x, y))
            {
                throw new ArgumentException("Coords out of bounds.");
            }
        }

        private void DecrementPosition(ref int x, ref int y)
        {
            x--;
            y--;
        }

        private HashSet<Tuple<int, int>> GetItemNeighbors(int x, int y)
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
                    if (x + i < Width && x + i >= 0 && y + j < Height && y + j >= 0)
                    {
                        neighbours.Add(new Tuple<int, int>(x + i, y + j));
                    }
                }
            }

            return neighbours;
        }

        private bool CheckBoardBoundaries(int x, int y)
        {
            return x < Width && x >= 0 && y < Height && y >=0; 
        }

        private bool CheckBoardInitBoundaries(int width, int height)
        {
            return width >= 3 && height >= 3 && width <= 50 && height <= 50;
        }
    }
}