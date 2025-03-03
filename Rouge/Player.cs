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

        //TODO
        //public Inventory Inventory { get; set; }


        //Staty gracza
        public int Power { get; set; }  // Siła
        public int Agility { get; set; }  // Zręczność
        public int Health { get; set; }  // Wytrzymałość
        public int Luck { get; set; }  // Szczęście
        public int Attack { get; set; }  // Atak
        public int Wisdom { get; set; }  // Madrosc
        public Player(int x, int y, int p, int a, int h, int l, int attack, int w)
        {
            X = x;
            Y = y;
            //TODO:
            //Inventory = new Inventory();

            // Inicjalizacja statystyk
            Power = p;
            Agility = a;
            Health = h;
            Luck = l;
            Attack = attack;
            Wisdom = w;
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
        public void DisplayStats()
        {
            Console.WriteLine("Statystyki Wiedzmina:");
            foreach(var property in typeof(Player).GetProperties())
            {
                if(property.PropertyType == typeof(int))
                {
                    Console.WriteLine($"{property.Name}: {property.GetValue(this)}");
                }
            }
        }
    }
}
