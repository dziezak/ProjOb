using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rouge.Items;

namespace Rouge
{
    internal class Inventory
    {
        public List<IItem> Items;
        public IItem? LeftHand { get; set; }
        public IItem? RightHand { get; set; }

        public Inventory()
        {
            Items = new List<IItem>();
            LeftHand = null;
            RightHand = null;
        }

        public void EquipItemLeftHand(int i, Player player)
        {
            if(i >= 0 && i < Items.Count)
            {
                if (Items[i].TwoHanded() == true)
                {
                    if (LeftHand == null && RightHand == null)
                    {
                        LeftHand = Items[i];
                        RightHand = Items[i];
                        Items.RemoveAt(i);
                    }
                    else 
                    {
                        player.WarningMessage += "Nie mozna trzymac dwurecznej broni w jednej rece\n";
                    }
                }
                else if (Items[i].TwoHanded() == false)
                if (LeftHand == null)
                {
                    LeftHand = Items[i];
                    Items.RemoveAt(i);
                }
            }
            else
            {
                player.WarningMessage += "Nie ma przedmiotu w inventory na miejscu i\n";
            }
        }
        public void EquipItemRightHand(int i, Player player)
        {
            if(i >= 0 && i < Items.Count)
            {
                if (Items[i].TwoHanded() == true)
                {
                    if (LeftHand == null && RightHand == null)
                    {
                        LeftHand = Items[i];
                        RightHand = Items[i];
                        Items.RemoveAt(i);
                    }
                    else
                    {
                        player.WarningMessage += "Nie mozna trzymac dwurecznej broni w jednej rece\n";
                    }
                }
                else if (Items[i].TwoHanded() == false)
                if (RightHand == null)
                {
                    RightHand = Items[i];
                    Items.RemoveAt(i);
                }
            }
            else
            {
                player.WarningMessage += "Nie ma przedmiotu w inventory na miejscu i\n";
            }
        }

        public void AddItem(IItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(IItem item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
        }

        public List<IItem> GetItems()
        {
            return Items;
        }

    }
}
