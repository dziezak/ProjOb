using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class Item : IItem
    {
        public string Name { get; set; }
        public virtual bool TwoHanded() => false;
        public virtual bool Equipable() => true;

        public Item(string name)
        {
            Name = name;
        }
        public virtual void ApplyEffect(Player player)
        {
            // klasa jest jako wyjsciowa bez imprementacji
        }
        public virtual string GetName()
        {
            return Name;
        }
        public virtual int GetAttack()
        {
            return 0;
        }
        public virtual int GetLuck() => 0;
    }
}
