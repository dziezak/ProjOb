namespace Rouge
{
    internal class Game
    {
        private Room _room;
        private Player _player;
        private string _instruction;
        private string _legend;
        
        public Game()
        {
            InstructionBuilder instructionBuilder = new InstructionBuilder();
            LegendBuilder legendBuilder = new LegendBuilder();
            DungeonBuilder dungeonBuilder = new DungeonBuilder(40, 30);
            DungeonDirector director = new DungeonDirector();
            
            //director.BuildBasicDungeon(); // opcja 1: sam labirynt bez przedmiotow
            director.BuildFilledDungeonWithRooms(dungeonBuilder); //opcja 2: labirynt ze wszystkim mozliwym
            director.BuildFilledDungeonWithRooms(instructionBuilder); //opcja 2: labirynt ze wszystkim mozliwym
            director.BuildFilledDungeonWithRooms(legendBuilder);// opcja 2: labirynt ze wszystkim mozliwym

            _instruction = instructionBuilder.GetResult();
            _legend = legendBuilder.GetResult();
            _room = dungeonBuilder.GetResult();
            _player = new Player(0, 0, 10, 10, 10, 10, 10, 10, 0, 0);
        }
        public void Start()
        { 
            //Console.Write(_instruction);
            GameDisplay.Instance?.DisplayAvailableString(_instruction, _room.Width); //instrukcja na starcie
            Console.ReadKey();
            Console.Clear();
            GameDisplay.Instance?.DisplayStats(_room, _player); 
            GameDisplay.Instance?.DisplayLog(15, _room.Width);
            while (true)
            {
                GameDisplay.Instance?.DisplayAvailableString(_legend, _room.Width);
                GameDisplay.Instance?.RenderLabirynth(_room, _player);
                var key = Console.ReadKey();
                
                _player.GetKey(key, _room);
            }
        }
    }
}
