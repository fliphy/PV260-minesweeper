namespace MinesWeeper
{
    public interface IBoard
    {
        GameState PlayTurn(int x, int y);
        GameState PlaceFlag(int x, int y);
        void CreateBoard(int width, int height);
    }
}