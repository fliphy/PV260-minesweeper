using System.Collections.Generic;

namespace MinesWeeper
{
    public interface IBoard
    {
        List<List<Item>> GameBoard { get; set; }
        void PlayTurn(int x, int y);
        
        int State { get; set; }
    }
}