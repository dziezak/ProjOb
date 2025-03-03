using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Room
    {
        private int width, height;
        private char[,] grid;
        private Dictionary<(int, int), List<IItem>> itemMap;

        public Room(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new char[height, width];
            itemMap = new Dictionary<(int, int), List<IItem>>();
            GenerateRoom();
        }

        // TODO: ( obcenie jest Hard-Code:
        private void GenerateRoom()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if ((x % 3 == 0 && y % 3 == 0) || (y==1 && x == 10) ||
                        (y==height-1 ) || (x==1 && y == 4) || (x==0 && y==4)) 
                        grid[y, x] = '█';
                    else
                        grid[y, x] = ' ';
                }
            }
            grid[0, 0] = ' ';
        }

        public bool IsWalkable(int x, int y)
        {
            if(x < 0 || y < 0 || x >= width || y >= height)
            {
                //Console.WriteLine("Chcial wyjsc poza plansze");
                //Console.ReadKey();
                return false;
            }
            if (grid[y, x] == '█')
            {
                //Console.WriteLine("Gracz nie moze wchodzic w sciane!");
                //Console.ReadKey();
                return false;
            }
            return true;
        }

        public void Render(Player player)
        {
            Console.WriteLine($"Player position: {player.X}, {player.Y}");
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if(x == player.X && y == player.Y)
                    {
                        Console.Write('¶');
                    }
                    else
                    {
                        //if(y == 0) Console.WriteLine(" ");
                        Console.Write(grid[y, x]);
                    }
                } 
                Console.WriteLine();
            }
        }
    }
}
