using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Player
    {
        public int X {  get; set; }
        public int Y {  get; set; }
        public Player(int x, int y)
        {
            X = x;
            Y = y;
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

        }
    }
}
