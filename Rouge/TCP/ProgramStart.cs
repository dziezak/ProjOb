using System;
using Rouge;
using Rouge.TCP;

static class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;

        if (args.Length > 0)
        {
            if (args[0] == "--server")
            {
                int port = args.Length > 1 ? int.Parse(args[1]) : 5555;
                GameServer server = new GameServer(port);
                server.Start();
            }
            else if (args[0] == "--client")
            {
                string address = args.Length > 1 ? args[1] : "127.0.0.1";
                int port = args.Length > 2 ? int.Parse(args[2]) : 5555;
                GameClient client = new GameClient(address, port);
                client.StartListening();

                // Teraz uruchamiamy grę na kliencie
                Game game = new Game(client);
                game.Start();
            }
        }
        else
        {
            Console.WriteLine("Start as (S)erver or (C)lient?");
            char mode = Console.ReadKey().KeyChar;
            Console.Clear();
            if (mode == 'S') new GameServer(5555).Start();
            else if (mode == 'C')
            {
                GameClient client = new GameClient("127.0.0.1", 5555);
                client.StartListening();
                Game game = new Game(client);
                game.Start();
            }
            else Console.WriteLine("Invalid choice.");
        }
    }
}