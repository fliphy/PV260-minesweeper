using System.Collections.Generic;

namespace MinesWeeper
{
    public interface IBoard
    {
        List<List<Item>> GameBoard { get; set; }
        GameState PlayTurn(int x, int y);
        GameState PlaceFlag(int x, int y);
        void CreateBoard(int width, int height);
    }
}