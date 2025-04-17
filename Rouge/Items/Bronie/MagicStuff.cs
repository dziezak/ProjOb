using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items.Bronie
{
    internal class MagicStuff: Weapon, IMagic
    {
        public MagicStuff(string name, int damage) : base(name, damage)
        {
            name = "MagicStuff";
        }

        public override void Accept(IWeaponVisitor visitor, Attack attack)
        {
            visitor.VisitMagic(this, attack);
        }
    }
}
