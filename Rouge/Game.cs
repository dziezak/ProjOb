namespace Rouge
{
    internal class Game
    {
        private Room _room;
        private Player _player;
        
        public Game()
        {
            DungeonBuilder builder = new DungeonBuilder(20, 40);
            DungeonDirector director = new DungeonDirector(builder);
            
            //director.BuildBasicDungeon(); // opcja 1: sam labirynt bez przedmiotow
            director.BuildFilledDungeonWithRooms(); //opcja 2: labirynt ze wszystkim mozliwym
            
            _room = builder.GetResult();
            _player = new Player(0, 0, 10, 10, 10, 10, 10, 10, 0, 0);
        }
        public void Start()
        { 
            // na starcie raz wywolujemy DisplayStats(), bo _player.GetKey() wywoluje DispalStats()
            GameDisplay.Instance?.DisplayStats(_room, _player); 
            while (true)
            {
                GameDisplay.Instance?.DisplayAvailableKeys(_room.Width);
                GameDisplay.Instance?.RenderLabirynth(_room, _player);
                var key = Console.ReadKey();
                
                _player.GetKey(key, _room);
            }
        }
    }
}
