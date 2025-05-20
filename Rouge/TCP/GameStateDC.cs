namespace Rouge.TCP;
using System.Text.Json.Serialization;

public class GameStateDC
{
    [JsonPropertyName("players")]
    public List<PlayerDC> Players { get; set; } = new List<PlayerDC>();
    
    [JsonPropertyName("CurrentRoom")]
    public RoomDC CurrentRoom { get; set; }

    [JsonPropertyName("isGameOver")]
    public bool IsGameOver { get; set; }
    
    [JsonPropertyName("enemiesNearby")]
    public List<EnemyDC> EnemiesNearby { get; set; } = new List<EnemyDC>(); // 📌 Teraz tylko nazwy i pozycje wrogów
    
    [JsonPropertyName("isPlayerDead")]
    public bool[] IsPlayerDead { get; set; } // 📌 Status żywotności graczy

    [JsonPropertyName("turnQueue")]
    public List<int> TurnQueue { get; set; } // 📌 Konwersja `Queue<int>` na `List<int>` dla JSON

    public GameStateDC(GameState gameState)
    {
        //TODO: tutaj serializacja nie dziala Player
        Players = gameState.Players.Select(p => new PlayerDC(p)).ToList(); 
        CurrentRoom = new RoomDC(gameState.CurrentRoom);
        IsGameOver = gameState.IsGameOver;
        EnemiesNearby = gameState.CurrentRoom._enemiesMap.Select(e => new EnemyDC(e.Value)).ToList();
        IsPlayerDead = gameState.IsPlayerDead; 
        TurnQueue = gameState.TurnQueue.ToList(); 
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

    [JsonPropertyName("name")]
    public string Name { get; set; } // 📌 Klient potrzebuje tylko nazwy gracza
    
    [JsonPropertyName("items")]
    public List<string> Items { get; set; } // 📌 Teraz przesyłamy tylko nazwy przedmiotów
    
    [JsonPropertyName("stats")]
    public StatsDC BaseStats { get; set; }
    
    public PlayerDC(Player player)
    {
        Id = player.Id;
        X = player.X;
        Y = player.Y;
    }

    public PlayerDC()
    {
        Id = -1;
        X = -1;
        Y = -1;
        Name = "Unknown";
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
        Image = enemy.Image;
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
}


public class RoomDC
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }


    [JsonPropertyName("grid")]
    public char[][] Grid { get; set; }

    public RoomDC(Room room)
    {
        Width = room.Width;
        Height = room.Height;
        Grid = room._grid; 
    }
}


