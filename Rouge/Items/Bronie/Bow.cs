using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge.Items.Bronie
{
    internal class Bow: Weapon
    {
        public Bow(string name, int damage) : base(name, damage)
        {
            name = "Bow";
        }
    }
}
