using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    class ItemDecorator : IItem
    {
    //Bazowy dekorator:
        protected IItem DecoratedItem;
        public ItemDecorator(IItem item)
        {
            DecoratedItem = item;
        }

        public virtual string Name => DecoratedItem.Name;
        public virtual bool TwoHanded() => DecoratedItem.TwoHanded();
        public virtual bool Equipable() => DecoratedItem.Equipable();
        public virtual void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        
        public virtual int GetAttack() => DecoratedItem.GetAttack();
        public virtual int GetLuck() => DecoratedItem.GetLuck();
        public virtual string GetName() => DecoratedItem.GetName();
        public virtual bool isCurrency() => DecoratedItem.isCurrency();
        public virtual int GetValue() => DecoratedItem.GetValue();
    }

    //Dekorator dla przedmiotów zwiększających szczescie:
    class LuckyItemDecorator : ItemDecorator
    {
        public LuckyItemDecorator(IItem item) : base(item) { }
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);

        public override string GetName() => "Lucky_" + DecoratedItem.GetName();
        public override int GetLuck() => DecoratedItem.GetLuck() + 5;
    }

    // Dekorator dla przedmiotów zwiększających atak:
    class PowerfulItemDecorator : ItemDecorator
    {
        public PowerfulItemDecorator(IItem item) : base(item) { }

        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override int GetAttack() => DecoratedItem.GetAttack() + 5;

        public override string GetName() => "Powerful_" + DecoratedItem.GetName();
    }

    // Dekorator dla przedmiotów zmniejszających atak:
    class PitifulItemDecorator : ItemDecorator
    {
        public PitifulItemDecorator(IItem item) : base(item) { }
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override int GetAttack() => DecoratedItem.GetAttack() - 5;
        public override string GetName() => "Pityful_" + DecoratedItem.GetName();
    }

    // Dekorator dla przedmiotów ktory sprwia, ze jest dwureczny:
    class HeavyItemDecorator : ItemDecorator
    {
        public HeavyItemDecorator(IItem item) : base(item) { }
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override bool TwoHanded() => true;
        public override string GetName() => "Heavy_" + DecoratedItem.GetName();
    }

    // Dekorator dla przedmiotów ktory nie ma zadnego efektu:
    class UselessItemDecorator : ItemDecorator
    {
        public UselessItemDecorator(IItem item) : base(item) { }
        public override bool Equipable() => false;
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public override int GetAttack() => 0;
        public override string GetName() => "Useless_" + DecoratedItem.GetName();
    }
}
