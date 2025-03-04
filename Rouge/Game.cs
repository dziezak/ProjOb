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
            player = new Player(10, 5, 10, 0, 0, 0, 0, 0, 0, 0);
        }
        public void Start()
        {
            while (true)
            {
                //Console.Clear();
                Console.SetCursorPosition(0, 0);
                room.Render(player);
                player.DisplayStats(room.width);
                ConsoleKeyInfo key = Console.ReadKey();
                
                player.Move(key, room);
            }
        }
    }
}
