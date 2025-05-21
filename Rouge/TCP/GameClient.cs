using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Rouge;
using Rouge.Items;
using Rouge.TCP;

public class GameClient
{
    private TcpClient _client;
    private NetworkStream _stream;
    private int _playerId;
    public GameState GameState { get; private set; }
    public PlayerAction PlayerAction { get; private set; }

    public GameClient(string address, int port)
    {
        PlayerAction = null;
        _client = new TcpClient(address, port);
        _stream = _client.GetStream();
        GameState = new GameState(new Room(40, 30));
    }

    public void Start()
    {
        RecivePlayerId();
        
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
     
        while (!GameState.IsGameOver)
        {
            Console.WriteLine("Waiting for game update...");
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
                    // Najpierw deserializujemy JSON do GameStateDC
                    GameStateDC gameStateDC = JsonSerializer.Deserialize<GameStateDC>(fullMessage);
                    if(gameStateDC == null)
                    {
                        Console.WriteLine("Error: Deserialized GameStateDC is null.");
                        continue;
                    }
                
                    // Następnie konwertujemy GameStateDC do pełnego GameState
                    GameState = ConvertGameStateDCToGameState(gameStateDC);

                    Console.Clear();
                    GameDisplay.Instance?.RenderLabirynth(GameState, _playerId);
                    GameDisplay.Instance?.DisplayStats(GameState.CurrentRoom, GameState.Players[_playerId], false);
                    GameDisplay.Instance?.DisplayLog(16, GameState.CurrentRoom.Width);
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
            //GameDisplay.Instance?.DisplayAvailableString(_legend, gameState.CurrentRoom.Width);
            //GameDisplay.Instance?.RenderLabirynth(gameState.CurrentRoom, gameState.Players, MyPlayerID, gameState.IsPlayerDead);
            Console.WriteLine("Dear player please enter smth:");
            char key = Console.ReadKey().KeyChar;
            
            //chain.Handle(key, gameState.CurrentRoom, gameState.Players[MyPlayerID]);
            PlayerAction playerAction = new PlayerAction(_playerId, key);
            SendPlayerAction(playerAction);
            if (key == 'v')
            {
                GameState.IsGameOver = true;
            } 
        }
    }
    public void SendPlayerAction(PlayerAction action)
    {
        string json = JsonSerializer.Serialize(action) + "\n";
        Console.WriteLine("sending JSON: " + json);
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
        return newState;
    }
    
    private Player ConvertPlayerDCToPlayer(PlayerDC pdc)
    {
        Player p = new Player(); 
        p.Id = pdc.Id;
        p.X = pdc.X;
        p.Y = pdc.Y;
        p.BaseStats = ConvertStatsDCToStats(pdc.BaseStats);
        p.Inventory = ConvertInventoryDCToInventory(pdc.Inventory);
        return p;
    }
    
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
                        IItem item = ConvertItemDCToItem(itemDC);
                        items.Add(item);
                    }
                    room._itemMap.Add(position, items);
                }
            }
        }
        
        // Konwersja enemiesMap: Dictionary<string, EnemyDC> -> Dictionary<(int, int), Enemy>
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
                    // Konwertujemy EnemyDC na Enemy.
                    // Funkcję ConvertEnemyDCToEnemy należy zaimplementować zgodnie z Twoją logiką.
                    Enemy enemy = ConvertEnemyDCToEnemy(kv.Value);
                    room._enemiesMap.Add(position, enemy);
                }
            }
        }
        return room;
    }

}

