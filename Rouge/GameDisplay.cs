namespace Rouge;
public class GameDisplay
{
    public string CriticalInfo = "";
    private Queue<string> _logQueue = new Queue<string>(); //Log gracza do wyswietlenia
    private static GameDisplay? _instance; // bedzie jedyna instancja tej klasy
    private GameDisplay(){}
    public static GameDisplay? Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameDisplay();
            }
            return _instance;
        }
    }
    public void RenderLabirynth(Room room, Player player)
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < room.Height; y++)
        {
            for(int x = 0; x < room.Width; x++)
            {
                if(x == player.X && y == player.Y)
                {
                    Console.Write('¶');
                }
                else if (room._enemiesMap.ContainsKey((y,x)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (room._enemiesMap[(y, x)].GetName() != null)
                    {
                        Console.Write(room._enemiesMap[(y,x)].GetName()[0]);
                    }
                    Console.ResetColor();
                }
                else if (room._itemMap[(x, y)].Count > 0)
                {
                    if(room._itemMap[(x, y)][0].GetName() == "Gold" || room._itemMap[(x, y)][0].GetName() == "Coin")
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(room._itemMap[(x, y)][0].GetName()[0]);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(room._grid[y, x]);
                }
            } 
            Console.WriteLine();
        }
    }
    int _maxRows = 0;
    public void DisplayStats(Room room, Player player)
    {
        int mapWidth = room.Width;  
        int infoWidth = 55;
        int infoHeight = 100;
        char[,] infoGrid = new char[infoHeight, infoWidth];
        List<string> infoLines = new List<string>();
        List<Enemy> enemiesNearby = room.GetEnemiesNearBy(player.Y, player.X);
                    

        int row = 0;
        void AddText(string text)
        {
            text = text.PadRight(infoWidth);
            text = text.Substring(0, text.Length -1) + "|";
            infoLines.Add(text);
        }
       
        var leftHand = player.Inventory?.LeftHand;
        var rightHand = player.Inventory?.RightHand;
        int attackCounter = (leftHand?.GetAttack() ?? 0) + (rightHand?.GetAttack() ?? 0);
        if (leftHand != null && leftHand.TwoHanded())
        {
            attackCounter = leftHand.GetAttack();
        }
        int luckCounter = (leftHand?.GetLuck() ?? 0) + (rightHand?.GetLuck() ?? 0);
        if(leftHand != null && leftHand.TwoHanded())
        {
            luckCounter = leftHand.GetLuck();
        }
        Stats displatyStats = player.GetCurrentStats();
        AddText($"Action Counter: {player.ActionCounter}");
        AddText("====================================================");
        AddText("Witchers Attributes:");
        AddText($"Power: {displatyStats.Power + attackCounter}");
        AddText($"Agility: {displatyStats.Agility}");
        AddText($"Health: {displatyStats.Health}");
        AddText($"Luck: {displatyStats.Luck + luckCounter}");
        AddText($"Aggression: {displatyStats.Attack}");
        AddText($"Wisdom: {displatyStats.Wisdom}");
        AddText("====================================================");
        AddText($"Coins: {player.Coins}");
        AddText($"Gold: {player.Gold}");
        AddText("====================================================");
        AddText($"Right Hand: {(player.Inventory?.RightHand != null ? player.Inventory.RightHand.GetName() : "None")}");
        AddText($"Left Hand: {(player.Inventory?.LeftHand != null ? player.Inventory.LeftHand.GetName() : "None")}");
        AddText("====================================================");
        AddText($"Number of potions Applied: {player.AppliedPotions.Count}");
        AddText("====================================================");
        AddText("Inventory:");

        if(player.Inventory.Items.Count == 0 || player.Inventory == null)
        {
            AddText("Empty");
        }
        int index = 0;
        foreach (var item in player.Inventory.GetItems())
        {
            AddText($"item {index}: " + item.GetName());
            index++;
        }
        AddText("====================================================");
        if (player.ItemsToGetFromRoom != null)
        {
            AddText("Items on tile:");
            for (int i = 0; i < player.ItemsToGetFromRoom.Count; i++)
            {
                if(player.ItemsToGetFromRoom[i].IsCurrency())
                    AddText($"Item {i}: {player.ItemsToGetFromRoom[i].GetValue()}x {player.ItemsToGetFromRoom[i].GetName()}");
                else
                    AddText($"Item {i}: {player.ItemsToGetFromRoom[i].GetName()}");
            }
        }
        AddText("====================================================");
        AddText("Enemies nearby:");
        foreach (var item in enemiesNearby)
        {
           string name = item.GetName();
           Stats stats = item.GetStats();
           AddText($"Enemy name: {name} with Attack: {stats.Attack} and Health {stats.Health}");
        }
        AddText("====================================================");
        if (!string.IsNullOrEmpty(player.WarningMessage))
        {
            AddText(player.WarningMessage);
        }

        while(infoLines.Count < _maxRows)
        {
            infoLines.Add(new string(' ', infoWidth));
        }
        _maxRows = infoLines.Count;


        int cursorTop = 0;
        Console.SetCursorPosition(mapWidth + 5, cursorTop);
        foreach(var line in infoLines)
        {
            Console.SetCursorPosition(mapWidth + 5, cursorTop++);
            Console.Write(line);
        }
    }
    public void DisplayAvailableString(string input, int mapWidth)
    {
        int startColumn = mapWidth + 65;
        int cursorTop = 0;
        string[] lines = input.Split('\n');

        Console.SetCursorPosition(startColumn, cursorTop);
        foreach (var description in lines)
        {
            Console.SetCursorPosition(startColumn, cursorTop++);
            Console.Write(description);
        }
    }

    public void AddLogMessage(string logMessage)
    {
        string[] logLines = logMessage.Split('\n');
        foreach (var line in logLines)
        {
            _logQueue.Enqueue(line);
            if (_logQueue.Count > 10)
            {
                _logQueue.Dequeue();
            }
        }
    }
   
    public void DisplayLog(int lineToStart, int mapWidth)
    {
        int startColumn = mapWidth + 65;
        int cursorTop = lineToStart;

        Console.SetCursorPosition(startColumn, cursorTop);
        Console.WriteLine("╔══════════════════════════════╗");
        Console.SetCursorPosition(startColumn, cursorTop + 1);
        Console.WriteLine("║      📝 WHICHERS LOGS:       ║");
        Console.SetCursorPosition(startColumn, cursorTop + 2);
        Console.WriteLine("╚══════════════════════════════╝");

        cursorTop += 4;

        for (int i = 0; i < 10; i++)
        {
            Console.SetCursorPosition(startColumn, cursorTop + i);
            Console.Write(new string(' ', 50));
        }

        // Wyświetlanie logów w kolejności od najstarszego do najnowszego
        foreach (var log in _logQueue)
        {
            if (cursorTop >= lineToStart + 14) break;
            Console.SetCursorPosition(startColumn, cursorTop++);
            Console.WriteLine(log);
        }
    }

    public void DisplayMovementInformation(string input, Room room)
    {
        int startColum = 0;
        int cursorTop = room.Height + 1;
        
        Console.SetCursorPosition(startColum, cursorTop);
        Console.Write(new String(' ', room.Width));
        
        Console.SetCursorPosition(startColum, cursorTop);
        Console.Write(input);
    }

    public void GameOverDisplay(Room room)
    {
        Console.Clear();

        string message = " GAME OVER ";
        string border = new string('=', message.Length + 4);

        int centerX = (room.Width) - (border.Length / 2);
        int centerY = room.Height / 2;

        Console.SetCursorPosition(centerX, centerY - 1);
        Console.WriteLine(border);

        Console.SetCursorPosition(centerX, centerY);
        Console.WriteLine($"|{message}|");

        Console.SetCursorPosition(centerX, centerY + 1);
        Console.WriteLine(border);

        Console.SetCursorPosition(0, room.Height + 2);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    } 
}