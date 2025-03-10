using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rouge
{
    internal class Inventory
    {
        public List<IItem> items;
        public IItem? LeftHand { get; set; }
        public IItem? RightHand { get; set; }

        public Inventory()
        {
            items = new List<IItem>();
            LeftHand = null;
            RightHand = null;
        }

        public void EquipItemLeftHand(int i, Player player)
        {
            if(i >= 0 && i < items.Count)
            {
                if (items[i].TwoHanded() == true)
                {
                    if (LeftHand == null && RightHand == null)
                    {
                        LeftHand = items[i];
                        RightHand = items[i];
                        items.RemoveAt(i);
                    }
                    else 
                    {
                        player.warningMessage += "Nie mozna trzymac dwurecznej broni w jednej rece\n";
                    }
                }
                else if (items[i].TwoHanded() == false)
                if (LeftHand == null)
                {
                    LeftHand = items[i];
                    items.RemoveAt(i);
                }
            }
            else
            {
                player.warningMessage += "Nie ma przedmiotu w inventory na miejscu i\n";
            }
        }
        public void EquipItemRightHand(int i, Player player)
        {
            if(i >= 0 && i < items.Count)
            {
                if (items[i].TwoHanded() == true)
                {
                    if (LeftHand == null && RightHand == null)
                    {
                        LeftHand = items[i];
                        RightHand = items[i];
                        items.RemoveAt(i);
                    }
                    else
                    {
                        player.warningMessage += "Nie mozna trzymac dwurecznej broni w jednej rece\n";
                    }
                }
                else if (items[i].TwoHanded() == false)
                if (RightHand == null)
                {
                    RightHand = items[i];
                    items.RemoveAt(i);
                }
            }
            else
            {
                player.warningMessage += "Nie ma przedmiotu w inventory na miejscu i\n";
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
