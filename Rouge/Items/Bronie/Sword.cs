using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items.Bronie
{
    internal class Sword : Weapon, IHeavy
    {
        public Sword(string name, int damage) : base(name, damage)
        {
            name = "Sword";
        }
        public override void Accept(IWeaponVisitor visitor, Attack attack)
        {
            Console.WriteLine($"[ACCEPT] Sword accepted visitor with attackType: {attack.Type}");
            visitor.VisitHeavy(this, attack);
        }
        public bool IsWeapon() => true;
    }
}
