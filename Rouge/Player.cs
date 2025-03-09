﻿using System;
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
                    DisplayStats(room.width);
                    break;
                case ConsoleKey.A:
                    newX--;
                    DisplayStats(room.width);
                    break;
                case ConsoleKey.S:
                    DisplayStats(room.width);
                    newY++;
                    break;
                case ConsoleKey.D:
                    DisplayStats(room.width);
                    newX++;
                    break;
                case ConsoleKey.P:
                    Item_to_pick_up = Console.ReadKey();
                    if (char.IsDigit(Item_to_pick_up.KeyChar))
                    {
                        PickUpItem(int.Parse(Item_to_pick_up.KeyChar.ToString()));
                    }
                    else
                    {
                        warningMessage += "Invalid input. Please enter a digit.\n";
                    }
                    DisplayStats(room.width);
                    break;
                case ConsoleKey.R:
                    Item_to_pick_up = Console.ReadKey();
                    if(char.IsDigit(Item_to_pick_up.KeyChar))
                    {
                        hand_item = int.Parse(Item_to_pick_up.KeyChar.ToString());
                        inventory.EquipItemRightHand(hand_item, this);
                    }
                    DisplayStats(room.width);
                    break;
                case ConsoleKey.L:
                    Item_to_pick_up = Console.ReadKey();
                    if (char.IsDigit(Item_to_pick_up.KeyChar))
                    {
                        hand_item = int.Parse(Item_to_pick_up.KeyChar.ToString());
                        inventory.EquipItemLeftHand(hand_item, this);
                    }
                    DisplayStats(room.width);
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
                    DisplayStats(room.width);
                    break;
                case ConsoleKey.M:
                    var itemsToRemove = inventory.GetItems().ToList();
                    foreach (var item in itemsToRemove)
                    {
                        room.DropItem(X, Y, item);
                        inventory.RemoveItem(item);
                    }
                    break;
            }
            if(room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            items_to_get_from_room = room.GetItemsAt(X, Y);
            DisplayStats(room.width);
            warningMessage = "";
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


        int maxRows = 0;
        public void DisplayStats(int mapWidth)
        {
            int infoWidth = mapWidth + 40;
            int infoHeight = 100;
            char[,] infoGrid = new char[infoHeight, infoWidth];
            List<string> infoLines = new List<string>();

            int row = 0;
            void AddText(string text)
            {
                text = text.PadRight(infoWidth);
                infoLines.Add(text);
            }
           
            var leftHand = inventory?.LeftHand;
            var rightHand = inventory?.RightHand;
            int attackCounter = (leftHand?.GetAttack() ?? 0) + (rightHand?.GetAttack() ?? 0);
            int luckCounter = (leftHand?.GetLuck() ?? 0) + (rightHand?.GetLuck() ?? 0);
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

            if(inventory.items.Count == 0 || inventory == null)
            {
                AddText("Empty");
            }
            int index = 0;
            foreach (var item in inventory.GetItems())
            {
                AddText($"item {index}: " + item.GetName());
                index++;
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
            if (!string.IsNullOrEmpty(warningMessage))
            {
                AddText(warningMessage);
            }

            /*
            for (int i=infoLines.Count; i<=maxRows; i++)
            {
                AddText("");
            }
            */
            while(infoLines.Count < maxRows)
            {
                infoLines.Add(new string(' ', infoWidth));
            }
            maxRows = infoLines.Count;


            int cursorTop = 0;
            Console.SetCursorPosition(mapWidth + 5, cursorTop);
            foreach(var line in infoLines)
            {
                Console.SetCursorPosition(mapWidth + 5, cursorTop++);
                Console.Write(line);
            }
            //maxRows = Math.Max(maxRows, infoLines.Count);

        }
    }
}
