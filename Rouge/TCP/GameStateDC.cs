using Rouge.Items;

namespace Rouge.TCP;
using System.Text.Json.Serialization;

public class GameStateDC
{
    [JsonPropertyName("NumberOfPlayers")]
    public int NumberOfPlayers { get; set; }
    [JsonPropertyName("players")]
    public PlayerDC[] Players { get; set; }
    
    [JsonPropertyName("CurrentRoom")]
    public RoomDC CurrentRoom { get; set; }

    [JsonPropertyName("isGameOver")]
    public bool IsGameOver { get; set; }
    
    [JsonPropertyName("isPlayerDead")]
    public bool[] IsPlayerDead { get; set; }

    [JsonPropertyName("turnQueue")]
    public List<int> TurnQueue { get; set; } 
    
    [JsonPropertyName("MaxNumberOfPlayers")]
    public int MaxNumberOfPlayers { get; set; }
    
    [JsonPropertyName("logQueue")]
    public List<string> LogQueue { get; set; }

    public GameStateDC(GameState gameState, GameDisplay gameDisplay)
    {
        //TODO: tutaj serializacja nie dziala Player
        NumberOfPlayers = gameState.NumberOfPlayers;
        MaxNumberOfPlayers = 9;
        
        Players = new PlayerDC[MaxNumberOfPlayers];

        for (int i = 0; i < NumberOfPlayers; i++)
        {
            Players[i] = new PlayerDC(gameState.Players[i]);
        }
        
        CurrentRoom = new RoomDC(gameState.CurrentRoom);
        IsGameOver = gameState.IsGameOver;
        IsPlayerDead = gameState.IsPlayerDead; 
        TurnQueue = gameState.TurnQueue.ToList();
        LogQueue = gameDisplay._logQueue.ToList();
    }

    [JsonConstructor]
    public GameStateDC(int numberOfPlayers, int maxNumberOfPlayers, PlayerDC[] players, RoomDC currentRoom,
        bool isGameOver, bool[] isPlayerDead, List<int> turnQueue, List<string> logQueue)
    {
        NumberOfPlayers = numberOfPlayers;
        MaxNumberOfPlayers = maxNumberOfPlayers;
        Players = players;
        CurrentRoom = currentRoom;
        IsGameOver = isGameOver;
        IsPlayerDead = isPlayerDead;
        TurnQueue = turnQueue;
        LogQueue = logQueue;
    }
}

public class PlayerDC
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }
    
    [JsonPropertyName("inventory")]
    public InventoryDC Inventory { get; set; }
    
    [JsonPropertyName("stats")]
    public StatsDC BaseStats { get; set; }
    
    [JsonPropertyName("itemsToGetFromRoom")]
    public List<ItemDC> ItemsToGetFromRoom { get; set; }
    
    public PlayerDC(Player player)
    {
        Id = player.Id;
        X = player.X;
        Y = player.Y;
        BaseStats = new StatsDC(player.GetCurrentStats());
        Inventory = new InventoryDC(player.Inventory);
        ItemsToGetFromRoom = new List<ItemDC>(); //TUTAJ
        foreach (var item in player.ItemsToGetFromRoom)
        {
            ItemsToGetFromRoom.Add(new ItemDC(item));
        }
    }

    [JsonConstructor]
    public PlayerDC(int id, int x, int y, InventoryDC inventory, StatsDC baseStats, List<ItemDC> itemsToGetFromRoom)
    {
        Id = id;
        X = x;
        Y = y;
        Inventory = inventory;
        BaseStats = baseStats;
        ItemsToGetFromRoom = itemsToGetFromRoom;
    }
}


public class EnemyDC
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("x")]
    public int X { get; set; }

    [JsonPropertyName("y")]
    public int Y { get; set; }

    [JsonPropertyName("stats")]
    public StatsDC BaseStats { get; set; }
    
    [JsonPropertyName("image")]
    public string Image { get; set; }
    public EnemyDC(Enemy enemy)
    {
        Name = enemy.Name;
        X = enemy.X;
        Y = enemy.Y;
        BaseStats = new StatsDC(enemy.GetStats());
        Image = enemy.Image;
    }

    [JsonConstructor]
    public EnemyDC(string name, int x, int y, StatsDC baseStats, string image)
    {
        Name = name;
        X = x;
        Y = y;
        BaseStats = baseStats;
        Image = image;
    }
}

public class StatsDC
{
    [JsonPropertyName("power")]
    public int Power { get; set; }

    [JsonPropertyName("agility")]
    public int Agility { get; set; }

    [JsonPropertyName("health")]
    public int Health { get; set; }

    [JsonPropertyName("luck")]
    public int Luck { get; set; }

    [JsonPropertyName("attack")]
    public int Attack { get; set; }

    [JsonPropertyName("wisdom")]
    public int Wisdom { get; set; }

    public StatsDC(Stats stats)
    {
        Power = stats.Power;
        Agility = stats.Agility;
        Health = stats.Health;
        Luck = stats.Luck;
        Attack = stats.Attack;
        Wisdom = stats.Wisdom;
    }

    [JsonConstructor]
    public StatsDC(int power, int agility, int health, int luck, int attack, int wisdom)
    {
        Power = power;
        Agility = agility;
        Health = health;
        Luck = luck;
        Attack = attack;
        Wisdom = wisdom;
    }
}


public class RoomDC
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("grid")]
    public char[][] Grid { get; set; }
    [JsonPropertyName("itemMap")]
    public Dictionary<string, List<ItemDC>> ItemMap { get; set; }

    [JsonPropertyName("enemiesMap")]
    public Dictionary<string, EnemyDC> EnemiesMap { get; set; }

    public RoomDC(Room room)
    {
        Width = room.Width;
        Height = room.Height;
        Grid = room._grid; 
        if(room._itemMap != null)
        {
            ItemMap = new Dictionary<string, List<ItemDC>>();
            foreach (var kv in room._itemMap)
            {
                string key = $"{kv.Key.Item1},{kv.Key.Item2}";
                List<ItemDC> items = new List<ItemDC>();
                foreach(var item in kv.Value)
                {
                    items.Add(new ItemDC(item));
                }
                ItemMap.Add(key, items);
            }
        }
        else
        {
            ItemMap = new Dictionary<string, List<ItemDC>>();
        }

        if (room._enemiesMap != null)
        {
            EnemiesMap = new Dictionary<string, EnemyDC>();
            foreach (var kv in room._enemiesMap)
            {
                string key = $"{kv.Key.Item1},{kv.Key.Item2}";
                EnemiesMap.Add(key, new EnemyDC(kv.Value)); 
            }
        }
        else
        {
            EnemiesMap = new Dictionary<string, EnemyDC>();
        }
    }

    [JsonConstructor]
    public RoomDC(int width, int height ,char[][] grid, Dictionary<string, List<ItemDC>> itemMap, Dictionary<string, EnemyDC> enemiesMap)
    {
        Width = width;
        Height = height;
        Grid = grid;
        if(itemMap != null)
            ItemMap = itemMap;
        else
            ItemMap = new Dictionary<string, List<ItemDC>>();

        if (enemiesMap != null)
            EnemiesMap = enemiesMap;
        else
            EnemiesMap = new Dictionary<string, EnemyDC>();
    }
}

public class ItemDC
{
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    public ItemDC(IItem item) ///TODO to check if ok?
    {
        Name = item.GetName();
    }

    [JsonConstructor]
    public ItemDC(string name)
    {
        Name = name;
    }
}

public class InventoryDC
{
    [JsonPropertyName("LeftHand")]
    public ItemDC LeftHand { get; set; }
    
    [JsonPropertyName("RightHand")]
    public ItemDC RightHand { get; set; }
    
    [JsonPropertyName("Items")]
    public List<ItemDC> Items { get; set; }

    public InventoryDC(Inventory inventory)
    {
        if (inventory.LeftHand != null) LeftHand = new ItemDC(inventory.LeftHand);
        if (inventory.RightHand != null) RightHand = new ItemDC(inventory.RightHand);
        this.Items = new List<ItemDC>();
        foreach (var item in inventory.Items)
        {
            Items.Add(new ItemDC(item));
        }
    }

    [JsonConstructor]
    public InventoryDC(ItemDC leftHand, ItemDC rightHand, List<ItemDC> items)
    {
        LeftHand = leftHand;
        RightHand = rightHand;
        Items = items;
    }
}


