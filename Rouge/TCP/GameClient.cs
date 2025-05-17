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
        Task.Run(() => ListenForUpdates());
        Task.Run(() => HandleClient());
    }

    private void ListenForUpdates()
    {
        byte[] buffer = new byte[1024];
    
        int preRead = _stream.Read(buffer, 0, buffer.Length);
        string idString = Encoding.UTF8.GetString(buffer, 0, preRead).Trim();

        if (!int.TryParse(idString, out _playerId))
        {
            Console.WriteLine("Error: Could not parse player ID.");
            return;
        }
        Console.WriteLine($"Received player ID: {_playerId}");
        while (true)
        {
            int read = _stream.Read(buffer, 0, buffer.Length);
            if (read == 0) break; // handle exception

            string message = Encoding.UTF8.GetString(buffer, 0, read);
            GameState = JsonSerializer.Deserialize<GameState>(message);
        }
        _client.Close();
    }

    public void HandleClient()
    {
        while (!GameState.IsGameOver)
        {
            //GameDisplay.Instance?.DisplayAvailableString(_legend, gameState.CurrentRoom.Width);
            //GameDisplay.Instance?.RenderLabirynth(gameState.CurrentRoom, gameState.Players, MyPlayerID, gameState.IsPlayerDead);
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
        string json = JsonSerializer.Serialize(action);
        byte[] data = Encoding.UTF8.GetBytes(json);
        _stream.Write(data);
    }
}

