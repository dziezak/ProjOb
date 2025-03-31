using Rouge.ActionHandler;
using Rouge.ActionHandler.Handlers;

namespace Rouge
{
    internal class Game
    {
        private Room _room;
        private Player _player;
        private string _instruction;
        private string _legend;
        private IActionHandler chain;
        
        
        public Game()
        {
            InstructionBuilder instructionBuilder = new InstructionBuilder();
            LegendBuilder legendBuilder = new LegendBuilder();
            DungeonBuilder dungeonBuilder = new DungeonBuilder(40, 30);
            
            DungeonDirector director = new DungeonDirector();
            director.BuildFilledDungeonWithRooms(dungeonBuilder); //opcja 2: labirynt ze wszystkim mozliwym
            //director.BuildBasicDungeon(); // opcja 1: sam labirynt bez przedmiotow
            director.BuildFilledDungeonWithRooms(instructionBuilder); //opcja 2: labirynt ze wszystkim mozliwym
            director.BuildFilledDungeonWithRooms(legendBuilder);// opcja 2: labirynt ze wszystkim mozliwym

            _instruction = instructionBuilder.GetResult();
            _legend = legendBuilder.GetResult();
            _room = dungeonBuilder.GetResult();
            _player = new Player(0, 0, 10, 10, 10, 10, 10, 10, 0, 0);
            
            ChainBuilder chainBuilder = new ChainBuilder(_player, _room);
            director.BuildFilledDungeonWithRooms(chainBuilder);
            chainBuilder.AddHandler(new GuardHandler());
            chain = chainBuilder.GetResult();
        }
        public void Start()
        {
            bool isGameOver = false;
            //Console.Write(_instruction);
            GameDisplay.Instance?.DisplayAvailableString(_instruction, _room.Width); //instrukcja na starcie
            Console.ReadKey();
            Console.Clear();
            GameDisplay.Instance?.DisplayStats(_room, _player, false); 
            GameDisplay.Instance?.DisplayLog(15, _room.Width);
            while (!isGameOver)
            {
                GameDisplay.Instance?.DisplayAvailableString(_legend, _room.Width);
                GameDisplay.Instance?.RenderLabirynth(_room, _player);
                char key = Console.ReadKey().KeyChar;
                
                chain.Handle(key, _room, _player);
                if (key == 'v')
                {
                    isGameOver = true;
                }
            }
        }
    }
}
