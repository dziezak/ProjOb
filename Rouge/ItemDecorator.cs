using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    internal interface IItemDecorator : IItem
    {
        IItem BaseItem { get; }
    }

    class ItemDecorator : IItemDecorator
    {
        public IItem BaseItem { get; private set; }
        public ItemDecorator(IItem baseItem)
        {
            BaseItem = baseItem;
        }

        public virtual string Name => BaseItem.Name;
        public virtual void ApplyEffect(Player player)
        {
            BaseItem.ApplyEffect(player);
        }
    }

    class CuresedItemDecorator : ItemDecorator
    {
        public CuresedItemDecorator(IItem baseItem) : base(baseItem) { }
        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);
            //player.L -= 5;
        }
        public override string Name => $"Cursed {BaseItem.Name}";
    }
}
