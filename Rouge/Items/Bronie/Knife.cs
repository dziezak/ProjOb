using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge.Items.Bronie
{
    internal class Knife: Weapon
    {
        public Knife(string name, int damage) : base(name, damage)
        {
            name = "Knife";
        }
    }
}
