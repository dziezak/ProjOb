﻿namespace Rouge
{
    internal class Game
    {
        private Room _room;
        private Player _player;
        
        public Game()
        {
            DungeonBuilder builder = new DungeonBuilder(20, 40);
            DungeonDirector director = new DungeonDirector(builder);
            
            //director.BuildBasicDungeon(); // opcja1
            director.BuildFilledDungeonWithRooms(); //opcja2
            
            _room = builder.GetResult();
            _player = new Player(0, 0, 10, 10, 10, 10, 10, 10, 0, 0);
        }
        public void Start()
        { 
            while (true)
            {
                _player.DisplayAvailableKeys(_room.Width);
                _room.Render(_player);
                var key = Console.ReadKey();
                
                _player.GetKey(key, _room);
            }
        }
    }
}
