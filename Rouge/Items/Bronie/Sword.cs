﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge.Items.Bronie
{
    internal class Sword : Weapon
    {
        public Sword(string name, int damage) : base(name, damage)
        {
            name = "Sword";
        }
    }
}
