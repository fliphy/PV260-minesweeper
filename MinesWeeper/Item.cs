namespace MinesWeeper
{
    public class Item
    {
        public bool Revealed { get; set; }
        public short MinesArround { get; set; }
        public bool HasMine {get; set;}
        public bool HasFlag { get; set; }
    }
}
