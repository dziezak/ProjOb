using Rouge.ActionHandler;

namespace Rouge;

public class GameState
{
    public Player[] Players { get; set; }
    public Room CurrentRoom { get; set; }
    public Queue<int> TurnQueue { get; set; }
    public bool IsGameOver { get; set; }
    public bool[] IsPlayerDead { get; set; }
    private const int MaxNumberOfPlayers = 9;
    public int NumberOfPlayers;

    public GameState(Room room)
    {
        NumberOfPlayers = 0;
        Players = new Player[MaxNumberOfPlayers];
        CurrentRoom = room;
        TurnQueue = new Queue<int>();
        IsGameOver = false;
        IsPlayerDead = new bool[MaxNumberOfPlayers+1];
        for(int i=0; i<MaxNumberOfPlayers; i++)
            IsPlayerDead[i] = false;
    }

    public void AddPlayer(Player player)
    {
        player.Id = NumberOfPlayers;
        Players[NumberOfPlayers] = player;
        NumberOfPlayers++;
        TurnQueue.Enqueue(player.Id);
    }

    public void RemovePlayer(Player player)
    {
        IsPlayerDead[player.Id] = true;
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
