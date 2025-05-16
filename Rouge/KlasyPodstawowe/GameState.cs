namespace Rouge;

public class GameState
{
    public List<Player> Players { get; set; }
    public Room CurrentRoom { get; set; }
    public Queue<int> TurnQueue { get; set; }
    public bool IsGameOver { get; set; }
    public bool[] isPlayerDead { get; set; }
    public int maxNumerOfPlayers = 9;

    public GameState(Room room)
    {
        Players = new List<Player>();
        CurrentRoom = room;
        TurnQueue = new Queue<int>();
        IsGameOver = false;
        isPlayerDead = new bool[maxNumerOfPlayers+1];
        for(int i=0; i<maxNumerOfPlayers; i++)
            isPlayerDead[i] = false;
    }

    public void AddPlayer(Player player)
    {
        Players.Add(player);
        TurnQueue.Enqueue(player.Id);
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
        isPlayerDead[player.Id] = true;
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
