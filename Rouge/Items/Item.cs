using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items
{
    public class Item : IItem
    {
        public string Name { get; set; }
        public virtual bool TwoHanded() => false;
        public virtual bool Equipable() => true;
        public int GetAttackValue { get; set; } // to serialization

        public Item(string name)
        {
            Name = name;
        }
        public virtual void ApplyEffect(Player player)
        {
            throw new NotImplementedException();
        }

        public int DeserializedAttackValue() => GetAttackValue;
        public virtual string GetName() => Name;
        public virtual int GetAttack() => 0;
        public virtual int GetLuck() => 0;
        public virtual bool IsCurrency() => false;
        public virtual int GetValue() => 0;
        public virtual Stats GetBuff() => new Stats(0, 0, 0, 0, 0, 0);
        public virtual bool IsConsumable() => false;
        public bool IsWeapon() => false;
        public virtual bool IsActive(int currentActionCounter) => false;
        public virtual void Update(Player player){}
        public virtual void Subscribe(Player player) { }
        public virtual void Unsubscribe(Player player){}
        public string typeToSerialize = "";
    }
}
