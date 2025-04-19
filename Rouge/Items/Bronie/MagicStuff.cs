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
        public MagicStuff(string name, int damage) : base("MagicStuff", damage){}

        public override void Accept(IWeaponVisitor visitor, Attack attack)
        {
            //Console.WriteLine($"[ACCEPT] MagicStuff accepted visitor with attackType: {attack.Type}");
            visitor.VisitMagic(this, attack);
        }
        public bool IsWeapon() => true;
    }
}
