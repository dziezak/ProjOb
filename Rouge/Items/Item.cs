namespace Rouge.Items
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
            throw new NotImplementedException();
        }
        public virtual string GetName() => Name;
        public virtual int GetAttack() => 0;
        public virtual int GetLuck() => 0;
        public virtual bool IsCurrency() => false;
        public virtual int GetValue() => 0;
        public virtual Stats GetBuff() => new Stats(0, 0, 0, 0, 0, 0);
        public virtual bool IsConsumable() => false;
        public virtual bool IsActive(int currentActionCounter) => false;
        public virtual void Update(){}
        
    }
}
