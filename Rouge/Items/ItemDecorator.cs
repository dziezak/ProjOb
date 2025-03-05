﻿using System;
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
        public virtual void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        
        public virtual int GetAttack() => DecoratedItem.GetAttack();
        public virtual int GetLuck() => DecoratedItem.GetLuck();
        public virtual string GetName() => DecoratedItem.GetName();
    }

    //Dekorator dla przedmiotów zwiększających szczescie:
    class LuckyItemDecorator : ItemDecorator
    {
        public LuckyItemDecorator(IItem item) : base(item) { }
        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);

        public override string GetName() => "Lucky_" + DecoratedItem.GetName();
    }

    // Dekorator dla przedmiotów zwiększających atak:
    class PowerfulItemDecorator : ItemDecorator
    {
        public PowerfulItemDecorator(IItem item) : base(item) { }

        public override void ApplyEffect(Player player) => DecoratedItem.ApplyEffect(player);
        public virtual int GetAttack() => DecoratedItem.GetAttack() + 5;

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
}
