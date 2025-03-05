using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Currency : Item
    {
        public int Value { get; set; }

        public Currency(string name, int value) : base(name)
        {
            Value = value;
        }

        public override void ApplyEffect(Player player)
        {
            // Możemy dodać mechanikę dodawania monet do ekwipunku gracza.
        }
    }
}
