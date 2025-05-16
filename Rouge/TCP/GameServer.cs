namespace Rouge.TCP;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class GameServer
{
    private TcpListener _server;
    private List<TcpClient> _clients = new List<TcpClient>();
    private GameState _gameState;

    public GameServer(int port)
    {
        _server = new TcpListener(IPAddress.Any, port);
        _gameState = new GameState(new Room(40, 30));
    }

    public void Start()
    {
        _server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = _server.AcceptTcpClient();
            _clients.Add(client);
            Console.WriteLine("Player connected!");

            Task.Run(() => HandleClient(client));
        }
    }

    private void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int read = stream.Read(buffer, 0, buffer.Length);
            if (read == 0) break;

            string message = Encoding.UTF8.GetString(buffer, 0, read);
            Console.WriteLine($"Received action: {message}");

            ProcessPlayerAction(message);
            BroadcastGameState();
        }

        client.Close();
        _clients.Remove(client);
    }

    private void ProcessPlayerAction(string jsonAction)
    {
        PlayerAction action = JsonSerializer.Deserialize<PlayerAction>(jsonAction);
        Player player = _gameState.Players.FirstOrDefault(p => p.Id == action.PlayerId);
        if (player != null)
        {
            if (action.Type == "MoveDown") _gameState.Players[player.Id].Mo
            else if (action.Type == "Attack") _gameState.PlayerAttack(player, action.TargetId);
        }
    }

    private void BroadcastGameState()
    {
        string json = JsonSerializer.Serialize(_gameState);
        byte[] data = Encoding.UTF8.GetBytes(json);
        foreach (var client in _clients)
        {
            client.GetStream().Write(data);
        }
    }
}

