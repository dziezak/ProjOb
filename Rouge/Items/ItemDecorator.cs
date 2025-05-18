using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items
{
    abstract class ItemDecorator : IItem, IWeapon
    {
    //Bazowy dekorator:
        protected IItem DecoratedItem;
        public ItemDecorator(IItem item)
        {
            DecoratedItem = item;
            typeToSerialize = "ItemDecorator";
        }

        public virtual string Name => DecoratedItem.Name;
        public virtual bool TwoHanded() => DecoratedItem.TwoHanded();
        public virtual bool Equipable() => DecoratedItem.Equipable();
        public virtual void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        
        public virtual int GetAttack() => DecoratedItem.GetAttack();
        public virtual int GetLuck() => DecoratedItem.GetLuck();
        public virtual string GetName() => DecoratedItem.GetName();
        public virtual bool IsCurrency() => DecoratedItem.IsCurrency();
        public virtual int GetValue() => DecoratedItem.GetValue();

        public virtual void Update(Player player) => DecoratedItem.Update(player);
        public virtual void Subscribe(Player player) => DecoratedItem.Subscribe(player);
        public virtual void Unsubscribe(Player player) => DecoratedItem.Unsubscribe(player);
        public string typeToSerialize = "";

        //do visitor
        public virtual void Accept(IWeaponVisitor visitor, Attack attack)
        {
            if (DecoratedItem is IWeapon weapon)
            {
                weapon.Accept(visitor, attack);
            }
            else
            {
                visitor.VisitOther(DecoratedItem, attack);
            }
        }

        public bool IsWeapon() => DecoratedItem.IsWeapon();
        public int GetDefenseValue(Player player, AttackType type)
        {
            if (DecoratedItem is Weapon weapon)
            {
                return weapon.GetDefenseValue(player, type);
            }
            throw new NotImplementedException();
        }
        public int BaseDamage { get; }
        public Stats GetBuff() => DecoratedItem.GetBuff();
        public bool IsActive(int currentActionCounter) => DecoratedItem.IsActive(currentActionCounter);
        public bool IsConsumable() => false;
    }

    //Dekorator dla przedmiotów zwiększających szczescie:
    class LuckyItemDecorator : ItemDecorator
    {
        public LuckyItemDecorator(IItem item) : base(item)
        {
            typeToSerialize = "LuckyItemDecorator";
        }
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);

        public override string GetName() => "Lucky_" + DecoratedItem.GetName();
        public override int GetLuck() => DecoratedItem.GetLuck() + 5;
    }

    // Dekorator dla przedmiotów zwiększających atak:
    class PowerfulItemDecorator : ItemDecorator
    {
        public PowerfulItemDecorator(IItem item) : base(item)
        {
            typeToSerialize = "PowerfulItemDecorator";
        }

        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override int GetAttack() => DecoratedItem.GetAttack() + 5;

        public override string GetName() => "Powerful_" + DecoratedItem.GetName();
    }

    // Dekorator dla przedmiotów zmniejszających atak:
    class PitifulItemDecorator : ItemDecorator
    {
        public PitifulItemDecorator(IItem item) : base(item)
        {
            typeToSerialize = "PitifulItemDecorator";
        }
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override int GetAttack() => DecoratedItem.GetAttack() - 5;
        public override string GetName() => "Pityful_" + DecoratedItem.GetName();
    }

    // Dekorator dla przedmiotów ktory sprwia, ze jest dwureczny:
    class HeavyItemDecorator : ItemDecorator
    {
        public HeavyItemDecorator(IItem item) : base(item)
        {
            typeToSerialize = "HeavyItemDecorator";
        }
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override bool TwoHanded() => true;
        public override string GetName() => "Heavy_" + DecoratedItem.GetName();
    }

    // Dekorator dla przedmiotów ktory nie ma zadnego efektu:
    class UselessItemDecorator : ItemDecorator
    {
        public UselessItemDecorator(IItem item) : base(item)
        {
            typeToSerialize = "UselessItemDecorator";
        }
        public override bool Equipable() => false;
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override string GetName() => "Useless_" + DecoratedItem.GetName();
    }
}
