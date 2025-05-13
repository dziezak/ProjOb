using Rouge.Items;
using Rouge.Items.WeaponInterfaces;

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
    public void RenderLabirynth(Room room, List<Player> players, int myPlayerID, bool[] isPlayerDead)
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < room.Height; y++)
        {
            for(int x = 0; x < room.Width; x++)
            {
                bool isPlayerHere = false;
                foreach (var player in players)
                {
                    if (player.X == x && player.Y == y)
                    {
                        isPlayerHere = true;
                        if (isPlayerDead[player.Id])
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Wyświetlamy na czerwono
                        }
                        else
                        {
                            Console.ForegroundColor = (player.Id == myPlayerID) ? ConsoleColor.Green : ConsoleColor.Blue;
                        }
                        Console.Write(player.Id.ToString()[0]); // Wyświetlamy ID gracza
                        Console.ResetColor();
                        break; // Przerywamy pętlę po znalezieniu gracza
                    }
                }
            
                if (isPlayerHere) continue;
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
    public void DisplayStats(Room room, Player player, bool Eliksir)
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
        AddText($"Action Counter: {Timer.GetActionCounter()}");
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
        if(Eliksir == false){
            AddText("====================================================");

            if(player.Inventory.Items.Count == 0 || player.Inventory == null)
            {
                //AddText("Empty");
            }
            else
            {
                AddText("Inventory:");
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
               int defence = item.GetDefense();
               AddText($"{name} Attack:{stats.Attack}, Health:{stats.Health}, Defence: {defence}");
            }
            AddText("====================================================");
            if (!string.IsNullOrEmpty(player.WarningMessage))
            {
                AddText(player.WarningMessage);
            }
        }
        else
        {
            foreach (var eliksir in player.AppliedPotions)
            {
                var eliksirToPrint = (Potion)eliksir;
                AddText($"{eliksirToPrint.GetName()} for {eliksirToPrint.Duration}");
            }
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

    public void GameOverDisplay()
    {
        Console.Clear();

        string message = " GAME OVER ";
        string border = new string('=', message.Length + 4);

        int centerX = 25;//(room.Width) - (border.Length / 2);
        int centerY = 25;//room.Height / 2;

        Console.SetCursorPosition(centerX, centerY - 1);
        Console.WriteLine(border);

        Console.SetCursorPosition(centerX, centerY);
        Console.WriteLine($"|{message}|");

        Console.SetCursorPosition(centerX, centerY + 1);
        Console.WriteLine(border);

        Console.SetCursorPosition(0, 25+2);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    public void RenderBattleUI(Player player)
    {
        bool Eliksir = true;
        Console.Clear();
        
        int infoWidth = 55;
        int infoHeight = 100;
        char[,] infoGrid = new char[infoHeight, infoWidth];
        List<string> infoLines = new List<string>();
                    

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
        AddText($"Action Counter: {Timer.GetActionCounter()}");
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
        if (player.AppliedPotions.Count > 0)
        {
            AddText($"Number of potions Applied: {player.AppliedPotions.Count}");
            foreach (var eliksir in player.AppliedPotions)
            {
                var eliksirToPrint = (Potion)eliksir;
                AddText($"{eliksirToPrint.GetName()} for {eliksirToPrint.Duration}");
            }
        }

        while(infoLines.Count < _maxRows)
        {
            infoLines.Add(new string(' ', infoWidth));
        }
        _maxRows = infoLines.Count;

        int cursorTop = 1;
        Console.SetCursorPosition(0, 0);
        foreach(var line in infoLines)
        {
            Console.SetCursorPosition(0, cursorTop++);
            Console.Write(line);
        }
    }

    public void RenderHeathBar(int CurrentHealth, int MaxHealth, string name, bool ForPlayer)
    {
        int barWidth = 40;
        int filledBars = (int)((double)CurrentHealth/MaxHealth * barWidth);
        string healthBar = "Bruh";
        if(filledBars >= 0 )
            healthBar = name + "[" + new string('#', filledBars).PadRight(barWidth, '_') + "]";

        int x = name.ToLower() == "player" ? 0 : 56;
        if (ForPlayer) x = 0;
        int y = 0;
        Console.SetCursorPosition(x, y);
        Console.Write(healthBar);
    }

    public void DisplayAvailableAttacks(Player player)
    {
        Console.SetCursorPosition(30, 30);
        Console.WriteLine("\nAvailable Attacks:");

        string format = "{0,-15} | Damage: {1,3} | Defense: {2, 3}";
        int leftBase = player.Inventory.LeftHand?.GetAttack() ?? 0;
        int rightBase = player.Inventory.RightHand?.GetAttack() ?? 0;


        // Tworzymy obiekty ataku, aby Visitor poprawnie obliczył rzeczywiste obrażenia
        Attack normalLeft = new Attack(AttackType.Heavy, leftBase, player);
        Attack normalRight = new Attack(AttackType.Heavy, rightBase, player);
        Attack stealthLeft = new Attack(AttackType.Stealth, leftBase, player);
        Attack stealthRight = new Attack(AttackType.Stealth, rightBase, player);
        Attack magicLeft = new Attack(AttackType.Magic, leftBase, player);
        Attack magicRight = new Attack(AttackType.Magic, rightBase, player);

        // Zastosowanie Visitor dla każdej broni

        if (player.Inventory?.LeftHand != null)
        {
            normalLeft.Apply((IWeapon)player.Inventory.LeftHand);
            stealthLeft.Apply((IWeapon)player.Inventory.LeftHand);
            magicLeft.Apply((IWeapon)player.Inventory.LeftHand);
        }

        if (player.Inventory?.RightHand != null)
        {
            normalRight.Apply((IWeapon)player.Inventory.RightHand);
            stealthRight.Apply((IWeapon)player.Inventory.RightHand);
            magicRight.Apply((IWeapon)player.Inventory.RightHand);
        }

        // Wyświetlenie danych
        Console.WriteLine(format, "1 - Normal", normalLeft.Damage + normalRight.Damage, normalLeft.Defense + normalRight.Defense);
        Console.WriteLine(format, "2 - Stealth", stealthLeft.Damage + stealthRight.Damage, stealthLeft.Defense + stealthRight.Defense);
        Console.WriteLine(format, "3 - Magic", magicLeft.Damage + magicRight.Damage, magicLeft.Defense + magicRight.Defense);
    }

    
}