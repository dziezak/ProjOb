using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    internal class Weapon : Item
    {
        public int Damage { get; set; }
        public Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
        }

        public override void ApplyEffect(Player player)
        {
            player.Attack += Damage;
            //player.DisplayStats();
        }

    }
}
