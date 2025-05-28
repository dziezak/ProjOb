using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Rouge;
using Rouge.Items;
using Rouge.Items.WeaponInterfaces;
using Rouge.TCP;

public class GameClient
{
    private TcpClient _client;
    private NetworkStream _stream;
    private int _playerId;
    public string _instrIuction;
    public string _legend;
    public bool isFighting;
    public GameState GameState { get; private set; }
    public PlayerAction PlayerAction { get; private set; }

    public GameClient(string address, int port)
    {
        PlayerAction = null;
        _client = new TcpClient(address, port);
        _stream = _client.GetStream();
        GameState = new GameState(new Room(40, 30));
        isFighting = false;
    }

    public void Start()
    {
        RecivePlayerId();
        InstructionBuilder instructionBuilder = new InstructionBuilder();
        LegendBuilder legendBuilder = new LegendBuilder();
        DungeonBuilder dungeonBuilder = new DungeonBuilder(40, 30);

        var director = new DungeonDirector();
        director.BuildFilledDungeonWithRooms(dungeonBuilder); //opcja 2: labirynt ze wszystkim mozliwym
        director.BuildFilledDungeonWithRooms(instructionBuilder); //opcja 2: labirynt ze wszystkim mozliwym
        director.BuildFilledDungeonWithRooms(legendBuilder);// opcja 2: labirynt ze wszystkim mozliwym

        _instrIuction = instructionBuilder.GetResult();
        _legend = legendBuilder.GetResult();
        
        GameDisplay.Instance?.DisplayAvailableString(_instrIuction, 40);
        
        char key = Console.ReadKey().KeyChar; // wcisjij cos aby zaczac gre
        
        Task listenTask = Task.Run(() => ListenForUpdates());
        Task handleTask = Task.Run(() => HandleClient());
        
        Task.WaitAll(listenTask, handleTask);
        Console.WriteLine("Game has finished.");
    }

    private void RecivePlayerId()
    {
        byte[] bytes = new byte[1024];
        
        int preRead = _stream.Read(bytes, 0, bytes.Length);
        string idString = Encoding.UTF8.GetString(bytes, 0, preRead).Trim();

        if (!int.TryParse(idString, out _playerId))
        {
            Console.WriteLine("Error: Could not parse player ID ");
            return;
        }
        Console.WriteLine($"Player {_playerId} has been received");
    }

    private void ListenForUpdates()
    {
        byte[] buffer = new byte[1024];
        StringBuilder receivedMessage = new StringBuilder();

        Console.Clear();
        while (!GameState.IsGameOver)
        {
            int read = _stream.Read(buffer, 0, buffer.Length);
            if (read <= 0) continue;

            string partialMessage = Encoding.UTF8.GetString(buffer, 0, read);
            receivedMessage.Append(partialMessage);

            if (partialMessage.Contains("\n"))
            {
                string fullMessage = receivedMessage.ToString().Trim();
                receivedMessage.Clear();

                try
                {
                    GameStateDC gameStateDC = JsonSerializer.Deserialize<GameStateDC>(fullMessage);
                    if(gameStateDC == null)
                    {
                        Console.WriteLine("Error: Deserialized GameStateDC is null.");
                        continue;
                    }
                
                    GameState = ConvertGameStateDCToGameState(gameStateDC);

                    if (GameDisplay.Instance == null)
                    {
                        Console.WriteLine("Error: GameDisplay is null."); 
                    }

                    if (true) //TODO CHANGE WITH RENDERING BATLE UI
                    {
                        GameDisplay.Instance?.RenderLabirynth(GameState, _playerId);
                        GameDisplay.Instance?.DisplayStats(GameState.CurrentRoom, GameState.Players[_playerId], false);
                        GameDisplay.Instance?.DisplayAvailableString(_legend, 40);
                        GameDisplay.Instance?.DisplayLog(16, GameState.CurrentRoom.Width);
                    }
                }
                
                catch (Exception ex)
                {
                    Console.WriteLine($"Error message in deserialization: {ex.Message}");
                }
            }
        }
        _client.Close();
    }
    
    
    
    public void HandleClient()
    {
        while (!GameState.IsGameOver)
        {
            char key = Console.ReadKey().KeyChar;
            
            PlayerAction playerAction = new PlayerAction(_playerId, key);
            SendPlayerAction(playerAction);
            if (key == 'v')
            {
                GameState.IsPlayerDead[_playerId] = true;
            }
        }
    }
    public void SendPlayerAction(PlayerAction action)
    {
        string json = JsonSerializer.Serialize(action) + "\n";
        //Console.WriteLine("sending JSON: " + json);
        byte[] data = Encoding.UTF8.GetBytes(json);
        
        _stream.Write(data, 0, data.Length);
        _stream.Flush(); // nie wiem czy potrzebne
    }
    
    private GameState ConvertGameStateDCToGameState(GameStateDC dc)
    {
        GameState newState = new GameState();
        newState.NumberOfPlayers = dc.NumberOfPlayers;
        newState.Players = new Player[newState.MaxNumberOfPlayers];
        for (int i = 0; i < dc.NumberOfPlayers; i++)
        {
            newState.Players[i] = ConvertPlayerDCToPlayer(dc.Players[i]);
        }
        newState.CurrentRoom = ConvertRoomDCToRoom(dc.CurrentRoom);
        newState.IsGameOver = dc.IsGameOver;
        newState.IsPlayerDead = dc.IsPlayerDead;
        newState.TurnQueue = new Queue<int>(dc.TurnQueue);
        GameDisplay.Instance._logQueue = new Queue<string>(dc.LogQueue);
        return newState;
    }
    
    private Player ConvertPlayerDCToPlayer(PlayerDC pdc)
    {
        Player p = new Player(
            pdc.Id,
            pdc.X,
            pdc.Y,
            ConvertInventoryDCToInventory(pdc.Inventory),
            ConvertStatsDCToStats(pdc.BaseStats),
            null,
            0,
            0,
            null,
            pdc.IsFighting,
            pdc.CurrentHealth,
            ConvertEnemyDCToEnemy(pdc.SelectedEnemy),
            pdc.AvailableAttacks
            ); 
        
        //p.ItemsToGetFromRoom = pdc.ItemsToGetFromRoom.Select(itemDC => ConvertItemDCToIItem(itemDC)).ToList();
        foreach (ItemDC itemDC in pdc.ItemsToGetFromRoom)
        {
            p.ItemsToGetFromRoom.Add(ConvertItemDCToIItem(itemDC));
        }
        return p;
    }
    
    /*
    private Inventory ConvertInventoryDCToInventory(InventoryDC idc)
    {
        Inventory inv = new Inventory();
        if (idc.LeftHand != null)
            inv.LeftHand = new Item(idc.LeftHand.Name);
        if (idc.RightHand != null)
            inv.RightHand = new Item(idc.RightHand.Name);
        if (idc.Items != null)
        {
            inv.Items = new List<IItem>();
            foreach (var itemDC in idc.Items)
            {
                inv.Items.Add(new Item(itemDC.Name));
                Console.WriteLine(itemDC.Name);
            }
        }
        return inv;
    }
    */

    private Inventory ConvertInventoryDCToInventory(InventoryDC idc)
    {
        Inventory inv = new Inventory();

        if (idc.LeftHand != null)
            inv.LeftHand = ConvertItemDCToIItem(idc.LeftHand);

        if (idc.RightHand != null)
            inv.RightHand = ConvertItemDCToIItem(idc.RightHand);

        if (idc.Items != null)
        {
            inv.Items = new List<IItem>();
            foreach (var itemDC in idc.Items)
            {
                inv.Items.Add(ConvertItemDCToIItem(itemDC));
            }
        }

        return inv;
    }

    private Stats ConvertStatsDCToStats(StatsDC sdc)
    {
        return new Stats(sdc.Power, sdc.Agility, sdc.Health, sdc.Luck, sdc.Attack, sdc.Wisdom);
    }

    
    private Room ConvertRoomDCToRoom(RoomDC rdc)
    {
        Room room = new Room(rdc.Width, rdc.Height);

        room._grid = rdc.Grid; 

        if (rdc.ItemMap != null)
        {
            room._itemMap = new Dictionary<(int, int), List<IItem>>();
            foreach (var kv in rdc.ItemMap)
            {
                string[] keyParts = kv.Key.Split(',');
                if (keyParts.Length == 2 &&
                    int.TryParse(keyParts[0], out int x) &&
                    int.TryParse(keyParts[1], out int y))
                {
                    (int, int) position = (x, y);
                    List<IItem> items = new List<IItem>();
                    foreach (var itemDC in kv.Value)
                    {
                        // Konwersja pojedynczego ItemDC do IItem.
                        // Funkcję ConvertItemDCToIItem należy zaimplementować zgodnie z Twoją logiką.
                        Item item = new Item(itemDC.Name);
                        items.Add(item);
                    }
                    room._itemMap.Add(position, items);
                }
            }
        }
        
        if (rdc.EnemiesMap != null)
        {
            room._enemiesMap = new Dictionary<(int, int), Enemy>();
            foreach (var kv in rdc.EnemiesMap)
            {
                string[] keyParts = kv.Key.Split(',');
                if (keyParts.Length == 2 &&
                    int.TryParse(keyParts[0], out int x) &&
                    int.TryParse(keyParts[1], out int y))
                {
                    (int, int) position = (x, y);
                    Enemy enemy = ConvertEnemyDCToEnemy(kv.Value);
                    room._enemiesMap.Add(position, enemy);
                }
            }
        }
        return room;
    }

    public Enemy ConvertEnemyDCToEnemy(EnemyDC enemydc)
    {
        string name = enemydc.Name;
        Stats stats = ConvertStatsDCToStats(enemydc.BaseStats);
        Enemy retEnemy;
        switch (name)
        {
            case "Minion":
                retEnemy = new Minion();
                break;
            case "Xenomorph":
                retEnemy = new Xenomorph();
                break;
            default:
                retEnemy = new Zombie();
                break;
        }

        return retEnemy;
    }
    
    private IItem ConvertItemDCToIItem(ItemDC itemDC)
    {
        if (itemDC == null)
            return null;

        return new Item(itemDC.Name);
    }
}

