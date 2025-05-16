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
    public GameState GameState { get; private set; }

    public GameClient(string address, int port)
    {
        _client = new TcpClient(address, port);
        _stream = _client.GetStream();
        GameState = new GameState(new Room(40, 30));
    }

    public void StartListening()
    {
        Task.Run(() => ListenForUpdates());
    }

    private void ListenForUpdates()
    {
        byte[] buffer = new byte[1024];

        while (true)
        {
            int read = _stream.Read(buffer, 0, buffer.Length);
            if (read == 0) break;

            string message = Encoding.UTF8.GetString(buffer, 0, read);
            GameState = JsonSerializer.Deserialize<GameState>(message);
            Console.WriteLine("Received updated game state.");
        }

        _client.Close();
    }

    public void SendPlayerAction(PlayerAction action)
    {
        string json = JsonSerializer.Serialize(action);
        byte[] data = Encoding.UTF8.GetBytes(json);
        _stream.Write(data);
    }
}

