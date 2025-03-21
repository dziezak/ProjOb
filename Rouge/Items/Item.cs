﻿namespace Rouge.Items
{
    internal class Item : IItem
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
        public virtual string GetName() => Name;
        public virtual int GetAttack() => 0;
        public virtual int GetLuck() => 0;
        public virtual bool IsCurrency() => false;
        public virtual int GetValue() => 0;
    }
}
