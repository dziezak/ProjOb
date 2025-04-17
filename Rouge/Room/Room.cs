using Rouge.Items;
using Rouge.Items.Bronie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    public class Room
    {
        public int Width, Height;
        public char[,] _grid;
        public Dictionary<(int, int), List<IItem>> _itemMap;
        private Random _random = new Random();  
        public Dictionary<(int, int), Enemy> _enemiesMap;

        public Room(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            _enemiesMap = new Dictionary<(int, int), Enemy>();
            _grid = new char[height, width];
            _itemMap = new Dictionary<(int, int), List<IItem>>();
        }

        public void SetGridElement(int y, int x, char value)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                _grid[y, x] = value;
            }
        }

        public char GetGridElement(int y, int x)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                return  _grid[y, x];
            }
            return '█';
        }

        public void GenerateMoney(int count)
        {
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
                    DropItem(x, y, item);
                else
                    i--;
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
                    item = new MagicStuff("MagicStuff", 7);
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

        public void GenerateBoringWeapons(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(0, Width);
                int y = _random.Next(0, Height);
                if (_grid[y, x] == ' ')
                    DropItem(x, y, CreateRandomItem());
                else
                    i--;
            }
        }

        public void GenerateFunWeapons(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(0, Width);
                int y = _random.Next(0, Height);
                if(_grid[y, x] == ' ')
                    DropItem(x, y, GetRandomDecorator(CreateRandomItem()));
            }
            //HardCode to help Player
            DropItem(0, 0, GetRandomDecorator(CreateRandomItem()));
            DropItem(0, 0, GetRandomDecorator(CreateRandomItem()));
            DropItem(0, 0, GetRandomDecorator(GetRandomDecorator( CreateRandomItem()))); // tworzymy podwojnie udekorowany przedmiot
            DropItem(0, 0, new LuckyItemDecorator(new PowerfulItemDecorator(CreateRandomItem()))); // tworzymy podwojnie udekorowany przedmiot
        }
        
        IItem CreateRandomPotion()
        {
            Random rng = new Random();
            int itemType = rng.Next(0, 5);
            IItem item;
            switch (itemType)
            {
                case 0:
                    item = new LuckyPotion();
                    return item;
                case 1:
                    item = new PowerPotion();
                    return item;
                case 2:
                    item = new WisdomPotion();
                    return item;
                case 3:
                    item = new AttackPotion();
                    return item;
                case 4:
                    item = new ClarityPotion();
                    return item;
                default:
                    return null;
            }
        }

        public void GeneratePotions(int count)
        {
           for (int i = 0; i < count; i++)
           {
              int x = _random.Next(0, Width);
              int y = _random.Next(0, Height);
              if (_grid[y, x] == ' ')
                  DropItem(x, y, CreateRandomPotion());
              else --i;
           }
        }

        public Enemy CreateRandomEnemy()
        {
            Random rng = new Random();
            int enemyType = rng.Next(0, 3);
            Enemy enemy;
            switch (enemyType)
            {
                case 0:
                    enemy = new Minion();
                    return enemy;
                case 1:
                    enemy = new Zombie();
                    return enemy;
                case 2:
                    enemy = new Xenomorph();
                    return enemy;
            }
            return null;
        }

        public void GenerateEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                int x = _random.Next(0, Width);
                int y = _random.Next(0, Height);
                if (_grid[y, x] == ' ')
                    if (!_enemiesMap.ContainsKey((y, x)))
                        _enemiesMap.Add((y, x), CreateRandomEnemy());
                    else
                        i--;
                else 
                    i--;
            }
        }

        public List<Enemy> GetEnemiesNearBy(int y, int x)
        {
            List<Enemy> result = new List<Enemy>();
            if (_enemiesMap.ContainsKey((y - 1, x))) result.Add(_enemiesMap[(y-1, x)]);
            if (_enemiesMap.ContainsKey((y + 1, x))) result.Add(_enemiesMap[(y+1, x)]);
            if (_enemiesMap.ContainsKey((y, x-1))) result.Add(_enemiesMap[(y, x-1)]);
            if (_enemiesMap.ContainsKey((y, x+1))) result.Add(_enemiesMap[(y, x+1)]);
            return result;
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
            if (_enemiesMap.ContainsKey((y, x))) // na razie przeciwnicy nic nie robia i nie da sie przez nich przejsc
            {
                return false;
            }
            return true;
        }

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

       
    }
}
