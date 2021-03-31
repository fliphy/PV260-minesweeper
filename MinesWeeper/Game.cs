namespace MinesWeeper
{
    public class Game
    {
        private IBoard gameBoard;
        public GameState State { get; set; } = GameState.GameOff;
        
        public Game(IBoard board)
        {
            gameBoard = board;
        }

        public void StartGame(int width, int height)
        {
            gameBoard.CreateBoard(width, height);
        }

        public IBoard PlayTurn(int x, int y)
        {
            State = gameBoard.PlayTurn(x, y);
            return gameBoard;
        }
        
        public IBoard PlaceFlag(int x, int y)
        {
            State = gameBoard.PlaceFlag(x, y);
            return gameBoard;
        }
    }
}