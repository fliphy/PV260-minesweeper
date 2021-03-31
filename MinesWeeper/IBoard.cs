using System.Collections.Generic;

namespace MinesWeeper
{
    public interface IBoard
    {
        List<List<Item>> GameBoard { get; set; }
        int PlayTurn(int x, int y);
        void CreateBoard(int width, int height);
    }
}