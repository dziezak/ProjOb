using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Rouge;

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
                    GameState = JsonSerializer.Deserialize<GameState>(fullMessage);
                    Console.Clear();

                    GameDisplay.Instance?.RenderLabirynth(GameState, _playerId);
                    GameDisplay.Instance?.DisplayStats(GameState.CurrentRoom, GameState.Players[_playerId], false);
                    GameDisplay.Instance?.DisplayLog(16, GameState.CurrentRoom.Width);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error message{ex.Message}");
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
}

