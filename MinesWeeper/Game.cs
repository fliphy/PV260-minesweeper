namespace MinesWeeper
{
    public class Game
    {
        private IBoard gameBoard;
        
        public Game(IBoard board)
        {
            gameBoard = board;
        }

        public void StartGame(int width, int height)
        {
            gameBoard.CreateBoard(width, height);
        }
    }
}