namespace Rouge;

public class GameState
{
    public List<Player> Players { get; set; }
    public Room CurrentRoom { get; set; }
    public Queue<int> TurnQueue { get; set; }
    public bool IsGameOver { get; set; }

    public GameState(Room room)
    {
        Players = new List<Player>();
        CurrentRoom = room;
        TurnQueue = new Queue<int>();
        IsGameOver = false;
    }

    public void AddPlayer(Player player)
    {
        Players.Add(player);
        TurnQueue.Enqueue(player.Id);
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
        TurnQueue = new Queue<int>(TurnQueue.Where(id => id != player.Id));
    }

    public int GetCurrentTurn()
    {
        return TurnQueue.Peek(); // Zwraca ID aktualnego gracza
    }

    public void NextTurn()
    {
        int current = TurnQueue.Dequeue();
        TurnQueue.Enqueue(current);
    }
}
