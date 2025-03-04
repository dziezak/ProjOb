using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    internal class Inventory
    {
        private List<IItem> items;
        public Item LeftHand { get; set; }
        public Item RightHand { get; set; }

        public Inventory()
        {
            items = new List<IItem>();
            LeftHand = null;
            RightHand = null;
        }

        public void EquipItem(Item item, Room room, int x, int y)
        {
            if (RightHand == null)
            {
                RightHand = item;
            }
            else if (LeftHand == null)
            {
                LeftHand = item;
            }
            else
            {
                room.DropItem(x, y, item);
            }
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
            }
        }

        public List<IItem> GetItems()
        {
            return items;
        }

    }
}
