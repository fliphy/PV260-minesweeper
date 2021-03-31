using System;
using System.Collections.Generic;
using System.Linq;

namespace MinesWeeper
{
    public class Board: IBoard
    {
        private const int MIN_BOARD_BOUNDARY = 3; 
        private const int MAX_BOARD_BOUNDARY = 50;
        
        public List<List<Item>> GameBoard { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CurrentFlagCount { get; set; }
        public int MineCount { get; set; }
        public HashSet<Tuple<int, int>> MinesCoords;
        
        public void CreateBoard(int width, int height)
        {
            Width = width;
            Height = height;
            if (!CheckBoardInitBoundaries(width, height))
            {
                throw new ArgumentException("Invalid board boundaries");
            }
            InitializeBoard(width, height);
            SetUpMines(width, height);
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
            var random = new Random();
            var itemCount = width * height;
            var minCount = Convert.ToInt32(Math.Ceiling(0.2 * itemCount));
            var maxCount = Convert.ToInt32(Math.Floor(0.6 * itemCount));
            MineCount = random.Next(minCount, maxCount + 1);

            MinesCoords = GenerateMineCords(width, height);
            PlaceMines();      
        }

        private HashSet<Tuple<int, int>> GenerateMineCords( int width, int height)
        {
            var random = new Random();
            var mineCoords = new HashSet<Tuple<int, int>>();
            while (mineCoords.Count != MineCount)
            {
                var x = random.Next(width);
                var y = random.Next(height);
                mineCoords.Add(new Tuple<int, int>(x, y));
            }

            return mineCoords;
        }

        private void PlaceMines()
        {
            foreach (var (x, y) in MinesCoords)
            {
                GameBoard[x][y].HasMine = true;
                var neighbors = GetItemNeighbors(x, y);
                foreach (var (a, b) in neighbors)
                {
                    GameBoard[a][b].MinesArround++;
                }
            }
        }
        
        public GameState PlayTurn(int x, int y)
        {
            DecrementPosition(ref x, ref y);
            if(!CheckBoardBoundaries(x, y))
            {
                throw new ArgumentException("Cords out of bounds");
            }

            if (GameBoard[x][y].HasMine) return GameState.GameOver;
            GameBoard[x][y].Revealed = true;
            if (GameBoard[x][y].MinesArround == 0)
            {
                RevealAvailableArea(x, y);
            }
            
            return GameState.GameOn;
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

        public GameState PlaceFlag(int x, int y)
        {
            DecrementPosition(ref x, ref y);
            if (!CheckBoardBoundaries(x, y))
            {
                throw new ArgumentException("Coords out of bounds.");
            }

            if (!GameBoard[x][y].HasFlag && CurrentFlagCount == MineCount) return GameState.GameOn;

            GameBoard[x][y].HasFlag = !GameBoard[x][y].HasFlag;

            if (GameBoard[x][y].HasFlag)
            {
                CurrentFlagCount++;
            }

            if (CurrentFlagCount == MineCount)
            {
                return CheckWinCondition() ? GameState.GameWon : GameState.GameOn;
            }

            return GameState.GameOn;
        }

        public Item GetItem(int x, int y)
        {
            DecrementPosition(ref x, ref y);
            return GameBoard[x][y];
        }

        private void DecrementPosition(ref int x, ref int y)
        {
            x--;
            y--;
        }

        private bool CheckWinCondition()
        {
            return MinesCoords.All(mine => GameBoard[mine.Item1][mine.Item2].HasFlag);
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
            return width >= MIN_BOARD_BOUNDARY 
                   && height >= MIN_BOARD_BOUNDARY
                   && width <= MAX_BOARD_BOUNDARY 
                   && height <= MAX_BOARD_BOUNDARY;
        }
    }
}