namespace Rouge
{
    internal class Game
    {
        private Room room;
        private Player player;
        
        //Hard-Code wielkosc tablicy
        public Game()
        {
            room = new Room(20, 40);
            player = new Player(0, 0, 10, 10, 10, 10, 10, 10, 0, 0);
        }
        public void Start()
        { 
            player.DisplayAvailableKeys();
            while (true)
            {
                room.Render(player);
                ConsoleKeyInfo key = Console.ReadKey();
                
                player.GetKey(key, room);
            }
        }
    }
}
