using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Player
    {
        public int X {  get; set; }
        public int Y {  get; set; }
        public Inventory inventory { get; set; }
        //Staty gracza
        public int Power { get; set; }  // Siła
        public int Agility { get; set; }  // Zręczność
        public int Health { get; set; }  // Wytrzymałość
        public int Luck { get; set; }  // Szczęście
        public int Attack { get; set; }  // Atak
        public int Wisdom { get; set; }  // Madrosc
        public int Coins { get; set; } // Kasa
        public int Gold { get; set; } // Zloto
        List<IItem> items_to_get_from_room = new List<IItem>();
        ConsoleKeyInfo Item_to_pick_up;
        ConsoleKeyInfo Item_to_drop;
        int hand_item;
        public string warningMessage = "";


        public Player(int x, int y, int p, int a, int h, int l, int attack, int w, int coins, int gold)
        {
            X = x;
            Y = y;
            inventory = new Inventory();

            // Inicjalizacja statystyk
            Power = p;
            Agility = a;
            Health = h;
            Luck = l;
            Attack = attack;
            Wisdom = w;
            Coins = coins;
            Gold = gold;
        }
        public void GetKey(ConsoleKeyInfo key, Room room)
        {
            int newX = X, newY = Y;
            switch(key.Key)
            {
                case ConsoleKey.W:
                    newY--;
                    break;
                case ConsoleKey.A:
                    newX--;
                    break;
                case ConsoleKey.S:
                    newY++;
                    break;
                case ConsoleKey.D:
                    newX++;
                    break;
                case ConsoleKey.P:
                    Item_to_pick_up = Console.ReadKey();
                    PickUpItem(int.Parse(Item_to_pick_up.KeyChar.ToString()));
                    DisplayStats(room.width);
                    break;
                case ConsoleKey.R:
                    Item_to_pick_up = Console.ReadKey();
                    if(char.IsDigit(Item_to_pick_up.KeyChar))
                    {
                        hand_item = int.Parse(Item_to_pick_up.KeyChar.ToString());
                        inventory.EquipItemRightHand(hand_item, this);
                    }
                    break;
                case ConsoleKey.L:
                    Item_to_pick_up = Console.ReadKey();
                    if (char.IsDigit(Item_to_pick_up.KeyChar))
                    {
                        hand_item = int.Parse(Item_to_pick_up.KeyChar.ToString());
                        inventory.EquipItemLeftHand(hand_item, this);
                    }
                    break;
                case ConsoleKey.O:
                    Item_to_drop = Console.ReadKey();
                    if (Item_to_drop.KeyChar == 'r')
                    {
                        if(inventory.RightHand != null)
                        {
                            if(inventory.RightHand.TwoHanded())
                            {
                                inventory.LeftHand = null;
                            }
                            room.DropItem(X, Y, inventory.RightHand);
                            inventory.RightHand = null;
                        }
                        else
                        {
                            warningMessage += "Nie trzymasz nic w prawej rece\n";
                        }
                    }else if(Item_to_drop.KeyChar == 'l')
                    {
                        if (inventory.LeftHand != null)
                        {
                            if(inventory.LeftHand.TwoHanded())
                            {
                                inventory.RightHand = null;
                            }
                            room.DropItem(X, Y, inventory.LeftHand);
                            inventory.LeftHand = null;
                        }
                        else
                        {
                            warningMessage += "Nie trzymasz nic w lewej rece\n";
                        }
                    }

                    break;

            }
            if(room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            if(key.Key == ConsoleKey.E)
            {
                //TODO
                //room.PuckUpItem(this);
            }
            items_to_get_from_room = room.GetItemsAt(X, Y);
        }

        // przyszlosciowa funkcja dla efektow
        public void ApplyEffect(IItem item)
        {
            item.ApplyEffect(this);
        }

        void PickUpItem(int n)
        {
            if (n >= 0 && n <items_to_get_from_room.Count )
            {
                inventory.AddItem(items_to_get_from_room[n]);
                items_to_get_from_room.RemoveAt(n);
            }
        }

        public void DisplayStats(int mapWidth)
        {
            int infoWidth = mapWidth + 40;
            int infoHeight = 100;
            char[,] infoGrid = new char[infoHeight, infoWidth];

            for(int y = 0; y < infoHeight; y++)
            {
                for(int x = 0; x < infoWidth; x++)
                {
                    infoGrid[y, x] = ' ';
                }
            }

            int row = 0;
            int maxRows = 0;
            void AddText(string text)
            {
                for(int i=0; i<text.Length; i++)
                {
                    if(i< infoWidth) infoGrid[row, i] = text[i];
                }
                row++;
                maxRows = Math.Max(maxRows, row);
            }
            int attackCounter = 0;
            if(inventory.LeftHand != null)
                attackCounter = inventory.LeftHand.GetAttack();
            if(inventory.RightHand != null)
                attackCounter += inventory.RightHand.GetAttack();
            int luckCounter = 0;
            if(inventory.LeftHand != null)
                luckCounter = inventory.LeftHand.GetLuck();
            if(inventory.RightHand != null)
                luckCounter += inventory.RightHand.GetLuck();
            AddText($"Action Counter: {0}");
            AddText("=========================");
            AddText("Witchers Attributes:");
            AddText($"Power: {Power} + {attackCounter}");
            AddText($"Agility: {Agility}");
            AddText($"Health: {Health}");
            AddText($"Luck: {Luck} + {luckCounter}");
            AddText($"Aggression: {Attack}");
            AddText($"Wisdom: {Wisdom}");
            AddText("=========================");
            AddText($"Coins: {Coins}");
            AddText($"Gold: {Gold}");
            AddText("=========================");
            AddText($"Right Hand: {(inventory?.RightHand != null ? inventory.RightHand.GetName() : "None")}");
            AddText($"Left Hand: {(inventory?.LeftHand != null ? inventory.LeftHand.GetName() : "None")}");
            AddText("=========================");
            AddText("Inventory:");

            if(inventory.items.Count == 0)
            {
                AddText("Empty");
            }
            foreach (var item in inventory.GetItems())
            {
                AddText(item.GetName());
            }
            AddText("=========================");
            if(items_to_get_from_room != null)
            {
                AddText("Items on tile:");
                for (int i = 0; i < items_to_get_from_room.Count; i++)
                {
                    AddText($"Item {i}: {items_to_get_from_room[i].GetName()}");
                }
            }
            AddText("=========================");
            AddText(warningMessage);

            for(int i=row; i<maxRows; i++)
            {
                AddText("");
            }


            Console.SetCursorPosition(mapWidth + 5, 0);
            for(int y=0;  y<infoHeight; y++)
            {
                for(int x = 0; x<infoWidth; x++)
                {
                    Console.Write(infoGrid[y, x] );
                }
                if (mapWidth + 5 < Console.BufferWidth && Console.CursorTop + 1 < Console.BufferHeight)
                {
                    Console.SetCursorPosition(mapWidth + 5, Console.CursorTop + 1);
                }
                else
                {
                    Console.SetCursorPosition(0, Console.BufferHeight - 1);
                }
            }

        }
    }
}
