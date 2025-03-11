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
        public int Width, Height;
        private char[,] _grid;
        private Dictionary<(int, int), List<IItem>> _itemMap;
        private Random _random = new Random();  

        public Room(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            _grid = new char[height, width];
            _itemMap = new Dictionary<(int, int), List<IItem>>();
            GenerateRoom();
            GenerateItems();
            GenerateMoney();
        }

        private void GenerateMoney()
        {
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                int itemType = _random.Next(0, 2);
                int randomValue = _random.Next(1, 10);
                int x = _random.Next(0, Width);
                int y = _random.Next(0, Height);
                IItem item;
                
                if(itemType == 0)
                {
                    item = new Currency("Gold", randomValue);
                }
                else
                {
                    item = new Currency("Coin", randomValue);
                }
                if (_grid[y, x] == ' ')
                {
                    DropItem(x, y, item);
                }
            }
        }

        private IItem CreateRandomItem()
        {
            int itemType = _random.Next(0, 6);
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
            int itemType = _random.Next(0, 5);
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
                int x = _random.Next(0, Width);
                int y = _random.Next(0, Height);
                if(_grid[y, x] == ' ')
                    DropItem(x, y, GetRandomDecorator(CreateRandomItem()));
            }
            DropItem(1, 1, GetRandomDecorator(CreateRandomItem()));
            DropItem(1, 1, GetRandomDecorator(CreateRandomItem()));
            DropItem(1, 1, GetRandomDecorator(GetRandomDecorator( CreateRandomItem()))); // tworzymy podwojnie udekorowany przedmiot
            //DropItem(1, 1, LuckyItemDecorator(PowerfulItemDecorator(CreateRandomItem()))); // tworzymy podwojnie udekorowany przedmiot
            DropItem(1, 1, new LuckyItemDecorator(new PowerfulItemDecorator(CreateRandomItem()))); // tworzymy podwojnie udekorowany przedmiot
        }

        // TODO: ( obcenie jest Hard-Code) :
        private void GenerateRoom()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if ((x % 3 == 0 && y % 3 == 0) || (y==1 && x == 10) ||
                        (y==Height-1 ) || (x==1 && y == 4) || (x==0 && y==4)) 
                        _grid[y, x] = '█';
                    else 
                        _grid[y, x] = ' ';
                    _itemMap[(x, y)] = new List<IItem>();
                }
            }
            _grid[0, 0] = ' ';
        }

        public bool IsWalkable(int x, int y)
        {
            if(x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return false;
            }
            if (_grid[y, x] == '█')
            {
                return false;
            }
            return true;
        }

        //funkcja uzywana do rozmieszczania przedmiotow
        public void DropItem(int x, int y, IItem item)
        {
            if(!_itemMap.ContainsKey((x, y)))
            {
                _itemMap[(x, y)] = new List<IItem> ();
            }
            _itemMap[(x, y)].Add (item);
        }

        public List<IItem> GetItemsAt(int x, int y)
        {
            return _itemMap.ContainsKey((x, y)) ? _itemMap[(x, y)] : new List<IItem>();
        }

        public void Render(Player player)
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < Height; y++)
            {
                for(int x = 0; x < Width; x++)
                {
                    if(x == player.X && y == player.Y)
                    {
                        Console.Write('¶');
                    }
                    else if (_itemMap[(x, y)].Count > 0)
                    {
                        Console.Write(_itemMap[(x, y)][0].GetName()[0]);
                    }
                    else
                    {
                        Console.Write(_grid[y, x]);
                    }
                } 
                Console.WriteLine();
            }
        }
    }
}
