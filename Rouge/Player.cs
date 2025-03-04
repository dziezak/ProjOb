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
        public void Move(ConsoleKeyInfo key, Room room)
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
        }

        public void ApplyEffect(IItem item)
        {
            item.ApplyEffect(this);
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
            void AddText(string text)
            {
                for(int i=0; i<text.Length; i++)
                {
                    if(i< infoWidth) infoGrid[row, i] = text[i];
                }
                row++;
            }
            AddText($"Action Counter: {0}");
            AddText("=========================");
            AddText("Witchers Attributes:");
            AddText($"Power: {Power}");
            AddText($"Agility: {Agility}");
            AddText($"Health: {Health}");
            AddText($"Luck: {Luck}");
            AddText($"Aggression: {Attack}");
            AddText($"Wisdom: {Wisdom}");
            AddText("=========================");
            AddText($"Coins: {Coins}");
            AddText($"Gold: {Gold}");
            AddText("=========================");
            AddText($"Right Hand: {(inventory?.RightHand != null ? inventory.RightHand.Name : "None")}");
            AddText($"Left Hand: {(inventory?.LeftHand != null ? inventory.LeftHand.Name : "None")}");
            AddText("=========================");
            AddText("Inventory:");

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
