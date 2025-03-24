namespace Rouge;

public class GameDisplay
{
    private static GameDisplay _instance; // bedzie jedyna instancja tej klasy

    private GameDisplay(){}

    public static GameDisplay Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameDisplay();
            }
            return _instance;
        }
    }

    public void RenderLabirynth(Room room, Player player)
    {
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < room.Height; y++)
        {
            for(int x = 0; x < room.Width; x++)
            {
                if(x == player.X && y == player.Y)
                {
                    Console.Write('¶');
                }
                else if (room._enemiesMap.ContainsKey((y,x)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (room._enemiesMap[(y, x)].GetName() != null)
                    {
                        Console.Write(room._enemiesMap[(y,x)].GetName()[0]);
                    }
                    Console.ResetColor();
                }
                else if (room._itemMap[(x, y)].Count > 0)
                {
                    if(room._itemMap[(x, y)][0].GetName() == "Gold" || room._itemMap[(x, y)][0].GetName() == "Coin")
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(room._itemMap[(x, y)][0].GetName()[0]);
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(room._grid[y, x]);
                }
            } 
            Console.WriteLine();
        }
    }
}