﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Item : IItem
    {
        public string Name { get; set; }
        public Item(string name)
        {
            Name = name;
        }
        public virtual void ApplyEffect(Player player)
        {
            // klasa jest jako wyjsciowa bez imprementacji
        }
    }
}
