using Rouge.ActionHandler;
using Rouge.ActionHandler.Handlers;

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
    private string _instruction;
    private string _legend; 
    public Dictionary<int, IActionHandler> PlayerChains { get; private set; }
    private DungeonDirector director { get; set; }

    public GameServer(int port)
    {
        _server = new TcpListener(IPAddress.Any, port);
        _gameState = new GameState(new Room(40, 30));
        PlayerChains = new Dictionary<int, IActionHandler>();
    }

    public void Start()
    {
        _server.Start();
        Console.WriteLine("Server started...");
        
        InstructionBuilder instructionBuilder = new InstructionBuilder();
        LegendBuilder legendBuilder = new LegendBuilder();
        DungeonBuilder dungeonBuilder = new DungeonBuilder(40, 30);
            
        director = new DungeonDirector();
        director.BuildFilledDungeonWithRooms(dungeonBuilder); //opcja 2: labirynt ze wszystkim mozliwym
        director.BuildFilledDungeonWithRooms(instructionBuilder); //opcja 2: labirynt ze wszystkim mozliwym
        director.BuildFilledDungeonWithRooms(legendBuilder);// opcja 2: labirynt ze wszystkim mozliwym

        _instruction = instructionBuilder.GetResult();
        _legend = legendBuilder.GetResult();
            
        _gameState = new GameState(dungeonBuilder.GetResult());
            
        _gameState.CurrentRoom = dungeonBuilder.GetResult();

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
        //TODO tutuaj jeszcze trzeba podac miejsce dla ludzi aby sie pojawiali w dobrych miejscach oraz id
        Player player = new Player(_gameState.NumberOfPlayers, 0, _gameState.NumberOfPlayers,
            new Inventory(), new Stats(10, 10, 1000, 10, 10, 10), null,
            0, 0, null, false, 1000, null, "");
        _gameState.AddPlayer(player);
        
        ChainBuilder chainBuilder = new ChainBuilder(_gameState.Players[player.Id], _gameState.CurrentRoom);
        director.BuildFilledDungeonWithRooms(chainBuilder);
        chainBuilder.AddHandler(new GuardHandler());
        IActionHandler chain = chainBuilder.GetResult();
        
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        
        buffer = Encoding.ASCII.GetBytes(player.Id.ToString() + "\n");
        stream.Write(buffer, 0, buffer.Length); // wysylamy na starcie ID jakie dostaje jako gracz nasz klient
        stream.Flush(); // Czy potrzebne?
        
        StringBuilder recivedMessage = new StringBuilder();

        while (true)
        {
            int read = stream.Read(buffer, 0, buffer.Length);
            if (read == 0) break;

            string patrialMessage = Encoding.UTF8.GetString(buffer, 0, read);
            //Console.WriteLine($"Raw recived data: {patrialMessage}"); // nie ma co wyswietlac bo nieczytalne
            recivedMessage.Append(patrialMessage);

            if (patrialMessage.Contains("\n"))
            {
                string fullMessage = recivedMessage.ToString().Trim();
                recivedMessage.Clear();

                //Console.WriteLine($"Full recived data before parsing: {fullMessage}");
                
                try
                {
                    PlayerAction action = JsonSerializer.Deserialize<PlayerAction>(fullMessage);
                    //Console.WriteLine($"Received action: {action.Type}, form player {player.Id}");

                    
                    ProcessPlayerAction(chain, action);
                    BroadcastGameState();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception here: {ex.Message}");
                }
            }
        }

        client.Close();
        _clients.Remove(client);
    }

    private void ProcessPlayerAction(IActionHandler chain, PlayerAction action)
    {
        Player player = _gameState.Players.FirstOrDefault(p => p.Id == action.PlayerId);
    
        if (player == null) return;
    
        if (action != null)
        {
            char key = action.Type; // Pobieramy pierwszy znak typu akcji (np. 'W' dla "MoveUp")
            chain.Handle(key, _gameState.CurrentRoom, player);
        }

        // Obsługa zakończenia gry
        if (_gameState.NumberOfPlayers == 0)
        {
            _gameState.IsGameOver = true;
        }

        if (_gameState.IsPlayerDead[player.Id])
        {
            _gameState.RemovePlayer(player);
        }
    }
    
    private void BroadcastGameState()
    {
        if (_gameState == null)
        {
            Console.WriteLine("BLAD: Game state is null");
            return;
        }
        
        GameStateDC gameStateDC = new GameStateDC(_gameState, GameDisplay.Instance);
        string json = JsonSerializer.Serialize(gameStateDC) + "\n";
        
        //Console.WriteLine("JSON wyslany do klientow:\n" + json);
        
        byte[] data = Encoding.UTF8.GetBytes(json);
        foreach (var client in _clients)
        {
            client.GetStream().Write(data);
        }
    }
}

