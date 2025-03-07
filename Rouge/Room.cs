using Rouge.Items;
using Rouge.Items.Bronie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Room
    {
        public int width, height;
        private char[,] grid;
        private Dictionary<(int, int), List<IItem>> itemMap;
        private Random Random = new Random();  

        public Room(int width, int height)
        {
            this.width = width;
            this.height = height;
            grid = new char[height, width];
            itemMap = new Dictionary<(int, int), List<IItem>>();
            GenerateRoom();
            GenerateItems();
        }

        private IItem CreateRandomItem()
        {
            int itemType = Random.Next(0, 6);
            IItem item;
            switch (itemType)
            {
                case 0:
                    item = new Sword("Sword", 10);
                    return item;
                case 1:
                    item = new Knife("Knife", 5);
                    return item;
                case 2:
                    item = new Bow("Bow", 7);
                    return item;
                case 3:
                    item = new Item("Stick");
                    return item;
                case 4:
                    item = new Item("Mug");
                    return item;
                case 5:
                    item = new Item("Fork");
                    return item;
                default:
                    return null;
            }
        }
        
        private IItem GetRandomDecorator(IItem przedmiot)
        {
            int itemType = Random.Next(0, 5);
            switch (itemType)
            {
            case 0:
                return new LuckyItemDecorator(przedmiot);
            case 1:
                return new PowerfulItemDecorator(przedmiot);
            case 2:
                return new PitifulItemDecorator(przedmiot);
            case 3:
                return new HeavyItemDecorator(przedmiot);
            case 4: 
                return new UselessItemDecorator(przedmiot);
            default:
                return null;
            }
        }

        public void GenerateItems()
        {
            int count = 50;// TODO
            for (int i = 0; i < count; i++)
            {
                int x = Random.Next(0, width);
                int y = Random.Next(0, height);
                if(grid[y, x] == ' ')
                    DropItem(x, y, GetRandomDecorator(CreateRandomItem()));
            }
            DropItem(1, 1, GetRandomDecorator(CreateRandomItem()));
            DropItem(1, 1, GetRandomDecorator(CreateRandomItem()));
        }

        // TODO: ( obcenie jest Hard-Code) :
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
                    itemMap[(x, y)] = new List<IItem>();
                }
            }
            grid[0, 0] = ' ';
        }

        public bool IsWalkable(int x, int y)
        {
            if(x < 0 || y < 0 || x >= width || y >= height)
            {
                return false;
            }
            if (grid[y, x] == '█')
            {
                return false;
            }
            return true;
        }

        //funkcja uzywana do rozmieszczania przedmiotow
        public void DropItem(int x, int y, IItem item)
        {
            if(!itemMap.ContainsKey((x, y)))
            {
                itemMap[(x, y)] = new List<IItem> ();
            }
            itemMap[(x, y)].Add (item);
        }

        public List<IItem> GetItemsAt(int x, int y)
        {
            return itemMap.ContainsKey((x, y)) ? itemMap[(x, y)] : new List<IItem>();
        }

        public void Render(Player player)
        {
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    if(x == player.X && y == player.Y)
                    {
                        Console.Write('¶');
                    }
                    else if (itemMap[(x, y)].Count > 0)
                    {
                        Console.Write(itemMap[(x, y)][0].GetName()[0]);
                    }
                    else
                    {
                        Console.Write(grid[y, x]);
                    }
                } 
                Console.WriteLine();
            }
        }
    }
}
