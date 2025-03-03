using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class NonUsableItem : Item
    {
        public NonUsableItem(string name) : base(name) { }

        public override void ApplyEffect(Player player)
        {
            // Te przedmioty nie mają efektu, więc ta metoda będzie pusta.
        }
    }
}
