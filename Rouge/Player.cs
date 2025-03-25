using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Rouge.Items;

namespace Rouge
{
    public class Player
    {
        public int X {  get; set; }
        public int Y {  get; set; }
        public Inventory Inventory { get; set; }

        public int ActionCounter;
        public Stats BaseStats { get; set; }
        public Stats AppliedStats { get; set; }
        public List<IItem> AppliedPotions = new List<IItem>();
        public int Coins {  get; set; }
        public int Gold { get; set; }
        public List<IItem> ItemsToGetFromRoom = new List<IItem>();
        ConsoleKeyInfo _itemToPickUp;
        ConsoleKeyInfo _itemToDrop;
        int _handItem;
        public string WarningMessage = "";
        public string LogMessage = "";


        public Player(int x, int y, int p, int a, int h, int l, int attack, int w, int coins, int gold)
        {
            X = x;
            Y = y;
            Inventory = new Inventory();
            ActionCounter = 0;

            // Inicjalizacja statystyk
            BaseStats = new Stats(p, a, h, l, attack, w);
            Coins = coins;
            Gold = gold;
        }
        
        public void ShowStats(Room room, Player player)
        {
            GameDisplay.Instance?.DisplayStats(room, player);
        }
 
        
        public void GetKey(ConsoleKeyInfo key, Room room)
        {
            int newX = X, newY = Y;
            switch(key.Key)
            {
                case ConsoleKey.W:
                    newY--;
                    NextTurn();
                    LogMessage += "Witcher moved up.";
                    break;
                case ConsoleKey.A:
                    newX--;
                    NextTurn();
                    LogMessage += "Witcher moved left.";
                    break;
                case ConsoleKey.S:
                    newY++;
                    NextTurn();
                    LogMessage += "Witcher moved down.";
                    break;
                case ConsoleKey.D:
                    newX++;
                    NextTurn();
                    LogMessage += "Witcher moved right.";
                    break;
                case ConsoleKey.P:
                    _itemToPickUp = Console.ReadKey();
                    if (char.IsDigit(_itemToPickUp.KeyChar))
                    {
                        PickUpItem(int.Parse(_itemToPickUp.KeyChar.ToString()));
                    }
                    else
                    {
                        WarningMessage += "Invalid input. Please enter a digit.\n";
                    }
                    break;
                case ConsoleKey.R:
                    _itemToPickUp = Console.ReadKey();
                    if(char.IsDigit(_itemToPickUp.KeyChar))
                    {
                        _handItem = int.Parse(_itemToPickUp.KeyChar.ToString());
                        Inventory.EquipItemRightHand(_handItem, this);
                    }
                    break;
                case ConsoleKey.L:
                    _itemToPickUp = Console.ReadKey();
                    if (char.IsDigit(_itemToPickUp.KeyChar))
                    {
                        _handItem = int.Parse(_itemToPickUp.KeyChar.ToString());
                        Inventory.EquipItemLeftHand(_handItem, this);
                    }
                    break;
                case ConsoleKey.O:
                    _itemToDrop = Console.ReadKey();
                    if (_itemToDrop.KeyChar == 'r')
                    {
                        if(Inventory.RightHand != null)
                        {
                            if(Inventory.RightHand.TwoHanded())
                            {
                                Inventory.LeftHand = null;
                            }
                            LogMessage += $"Witcher dropped {Inventory.RightHand.GetName()}.";
                            room.DropItem(X, Y, Inventory.RightHand);
                            Inventory.RightHand = null;
                        }
                        else
                        {
                            WarningMessage += "Nie trzymasz nic w prawej rece\n";
                        }
                    }else if(_itemToDrop.KeyChar == 'l')
                    {
                        if (Inventory.LeftHand != null)
                        {
                            if(Inventory.LeftHand.TwoHanded())
                            {
                                Inventory.RightHand = null;
                            }
                            LogMessage += $"Witcher dropped {Inventory.LeftHand.GetName()}.";
                            room.DropItem(X, Y, Inventory.LeftHand);
                            Inventory.LeftHand = null;
                        }
                        else
                        {
                            WarningMessage += "Nie trzymasz nic w lewej rece\n";
                        }
                    }
                    break;
                case ConsoleKey.M:
                    var itemsToRemove = Inventory.GetItems().ToList();
                    LogMessage += $"Witcher dropped all his inventory.";
                    foreach (var item in itemsToRemove)
                    {
                        room.DropItem(X, Y, item);
                        Inventory.RemoveItem(item);
                    }
                    break;
                case ConsoleKey.E:
                    _itemToDrop = Console.ReadKey();
                    if (_itemToDrop.KeyChar == 'r')
                    {
                        if(Inventory.RightHand != null && Inventory.RightHand.IsConsumable())
                        { 
                            LogMessage += $"Witcher used {Inventory.RightHand.GetName()}.";
                            ApplyEffect(Inventory.RightHand); 
                            Inventory.RightHand = null;
                        }
                        else
                        {
                            WarningMessage += "nie ma w prawej recej nic uzywalnego\n";
                        }
                    }else if(_itemToDrop.KeyChar == 'l')
                    {
                        if (Inventory.LeftHand != null && Inventory.LeftHand.IsConsumable())
                        {
                            LogMessage += $"Witcher used {Inventory.LeftHand.GetName()}.";
                            ApplyEffect(Inventory.LeftHand);
                            Inventory.LeftHand = null;
                        }
                        else
                        {
                            WarningMessage += "nie ma w lewej recej nic uzywalnego\n";
                        }
                    }
                    break;
            }
            if(room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            ItemsToGetFromRoom = room.GetItemsAt(X, Y);
            ShowStats(room, this);
            WarningMessage = "";
            if(LogMessage.Length > 0)
                GameDisplay.Instance?.AddLogMessage(LogMessage);
            GameDisplay.Instance?.DisplayLog(15, room.Width);
            LogMessage = "";
        }

        // przyszlosciowa funkcja dla efektow
        public void ApplyEffect(IItem item)
        {
            item.ApplyEffect(this);
        }

        void PickUpItem(int n)
        {
            
            if (n >= 0 && n <ItemsToGetFromRoom.Count )
            {
                if (!ItemsToGetFromRoom[n].Equipable() )
                {
                    if (!ItemsToGetFromRoom[n].IsCurrency())
                    {
                        WarningMessage = "You can't equip this item, cause it's Unusable!\n";
                    }
                    else if (ItemsToGetFromRoom[n].GetName() == "Gold")
                    {
                        Gold += ItemsToGetFromRoom[n].GetValue();
                        LogMessage += $"Witcher picked up Gold.";
                        ItemsToGetFromRoom.RemoveAt(n);
                    }
                    else
                    {
                        Coins += ItemsToGetFromRoom[n].GetValue();
                        LogMessage += $"Witcher picked up Coins.";
                        ItemsToGetFromRoom.RemoveAt(n);
                    }
                }
                else
                {
                    LogMessage += $"Witcher picked up {ItemsToGetFromRoom[n].GetName()}.";
                    Inventory.AddItem(ItemsToGetFromRoom[n]);
                    ItemsToGetFromRoom.RemoveAt(n);
                }
            }
        }

        public void DrinkPotion(IItem potion)
        {
            AppliedPotions.Add(potion);
        }

        public Stats GetCurrentStats()
        {
            Stats currentStats = BaseStats;
            foreach (var potion in AppliedPotions)
            {
                if (potion.IsActive(ActionCounter))
                {
                    currentStats += potion.GetBuff();
                }
            }
            return currentStats;
        }

        public void NextTurn()
        {
            ActionCounter++;
            AppliedPotions.RemoveAll(potion => !potion.IsActive(ActionCounter));
        }

    }

}
