using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items.Bronie
{
    internal class Knife: Weapon, ILight
    {
        public Knife(string name, int damage) : base(name, damage)
        {
            name = "Knife";
        }

        public override void Accept(IWeaponVisitor visitor, Attack attack)
        {
            Console.WriteLine($"[ACCEPT] Knife accepted visitor with attackType: {attack.Type}");
            visitor.VisitLight(this, attack);
        }
        public bool IsWeapon() => true;
    }
}
