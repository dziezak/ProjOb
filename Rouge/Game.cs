using Rouge.ActionHandler;
using Rouge.ActionHandler.Handlers;

namespace Rouge
{
    public class Game
    {
        private string _instruction;
        private string _legend;
        private IActionHandler chain;
        public GameState gameState;
        public static bool isGameOver;
        public int MyPlayerID { get; set; }
        
        
        
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
            
            gameState = new GameState(dungeonBuilder.GetResult());
            
            gameState.CurrentRoom = dungeonBuilder.GetResult();
            Player player1 = new Player(0, 0, 10, 10, 100, 10, 10, 10, 10, 0);
            player1.Id = 1;
            gameState.AddPlayer(player1);
            
            Player player2 = new Player(3, 3, 10, 10, 100, 10, 10, 10, 10, 0);
            player2.Id = 2;
            gameState.AddPlayer(player2);
            
            MyPlayerID = player1.Id;
            
            ChainBuilder chainBuilder = new ChainBuilder(gameState.Players[0], gameState.CurrentRoom);
            director.BuildFilledDungeonWithRooms(chainBuilder);
            chainBuilder.AddHandler(new GuardHandler());
            chain = chainBuilder.GetResult();
        }
        public void Start()
        {
            
            gameState.IsGameOver = false;
            //Console.Write(_instruction);
            GameDisplay.Instance?.DisplayAvailableString(_instruction, gameState.CurrentRoom.Width); //instrukcja na starcie
            Console.ReadKey();
            Console.Clear();
            GameDisplay.Instance?.DisplayStats(gameState.CurrentRoom, gameState.Players[0], false); 
            GameDisplay.Instance?.DisplayLog(16, gameState.CurrentRoom.Width);
            while (!gameState.IsGameOver)
            {
                if (isGameOver == true) gameState.IsGameOver = true;
                GameDisplay.Instance?.DisplayAvailableString(_legend, gameState.CurrentRoom.Width);
                GameDisplay.Instance?.RenderLabirynth(gameState.CurrentRoom, gameState.Players[0]);
                char key = Console.ReadKey().KeyChar;
                
                chain.Handle(key, gameState.CurrentRoom, gameState.Players[0]);
                if (key == 'v')
                {
                    gameState.IsGameOver = true;
                }
            }
        }
    }
}
