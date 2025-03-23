using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Rouge.Items;

namespace Rouge
{
    public class Player
    {
        public int X {  get; set; }
        public int Y {  get; set; }
        public Inventory Inventory { get; set; }
        //Staty gracza

        public int ActionCounter;
        public Stats BaseStats { get; set; }
        public Stats AppliedStats { get; set; }
        public List<IItem> AppliedPotions = new List<IItem>();
        public int Coins {  get; set; }
        public int Gold { get; set; } 
        List<IItem> _itemsToGetFromRoom = new List<IItem>();
        ConsoleKeyInfo _itemToPickUp;
        ConsoleKeyInfo _itemToDrop;
        int _handItem;
        public string WarningMessage = "";


        public Player(int x, int y, int p, int a, int h, int l, int attack, int w, int coins, int gold)
        {
            X = x;
            Y = y;
            Inventory = new Inventory();
            ActionCounter = 0;

            // Inicjalizacja statystyk
            BaseStats = new Stats(p, a, h, l, attack, w);
            Coins = coins;
            Gold = gold;
        }
        public void GetKey(ConsoleKeyInfo key, Room room)
        {
            int newX = X, newY = Y;
            switch(key.Key)
            {
                case ConsoleKey.W:
                    newY--;
                    NextTurn();
                    DisplayStats(room.Width);
                    break;
                case ConsoleKey.A:
                    newX--;
                    NextTurn();
                    DisplayStats(room.Width);
                    break;
                case ConsoleKey.S:
                    DisplayStats(room.Width);
                    NextTurn();
                    newY++;
                    break;
                case ConsoleKey.D:
                    DisplayStats(room.Width);
                    NextTurn();
                    newX++;
                    break;
                case ConsoleKey.P:
                    _itemToPickUp = Console.ReadKey();
                    if (char.IsDigit(_itemToPickUp.KeyChar))
                    {
                        PickUpItem(int.Parse(_itemToPickUp.KeyChar.ToString()));
                    }
                    else
                    {
                        WarningMessage += "Invalid input. Please enter a digit.\n";
                    }
                    DisplayStats(room.Width);
                    break;
                case ConsoleKey.R:
                    _itemToPickUp = Console.ReadKey();
                    if(char.IsDigit(_itemToPickUp.KeyChar))
                    {
                        _handItem = int.Parse(_itemToPickUp.KeyChar.ToString());
                        Inventory.EquipItemRightHand(_handItem, this);
                    }
                    DisplayStats(room.Width);
                    break;
                case ConsoleKey.L:
                    _itemToPickUp = Console.ReadKey();
                    if (char.IsDigit(_itemToPickUp.KeyChar))
                    {
                        _handItem = int.Parse(_itemToPickUp.KeyChar.ToString());
                        Inventory.EquipItemLeftHand(_handItem, this);
                    }
                    DisplayStats(room.Width);
                    break;
                case ConsoleKey.O:
                    _itemToDrop = Console.ReadKey();
                    if (_itemToDrop.KeyChar == 'r')
                    {
                        if(Inventory.RightHand != null)
                        {
                            if(Inventory.RightHand.TwoHanded())
                            {
                                Inventory.LeftHand = null;
                            }
                            room.DropItem(X, Y, Inventory.RightHand);
                            Inventory.RightHand = null;
                        }
                        else
                        {
                            WarningMessage += "Nie trzymasz nic w prawej rece\n";
                        }
                    }else if(_itemToDrop.KeyChar == 'l')
                    {
                        if (Inventory.LeftHand != null)
                        {
                            if(Inventory.LeftHand.TwoHanded())
                            {
                                Inventory.RightHand = null;
                            }
                            room.DropItem(X, Y, Inventory.LeftHand);
                            Inventory.LeftHand = null;
                        }
                        else
                        {
                            WarningMessage += "Nie trzymasz nic w lewej rece\n";
                        }
                    }
                    DisplayStats(room.Width);
                    break;
                case ConsoleKey.M:
                    var itemsToRemove = Inventory.GetItems().ToList();
                    foreach (var item in itemsToRemove)
                    {
                        room.DropItem(X, Y, item);
                        Inventory.RemoveItem(item);
                    }
                    break;
                case ConsoleKey.E:
                    _itemToDrop = Console.ReadKey();
                    if (_itemToDrop.KeyChar == 'r')
                    {
                        if(Inventory.RightHand != null && Inventory.RightHand.IsConsumable())
                        {
                           ApplyEffect(Inventory.RightHand); 
                           Inventory.RightHand = null;
                        }
                        else
                        {
                            WarningMessage += "nie ma w prawej recej nic uzywalnego\n";
                        }
                    }else if(_itemToDrop.KeyChar == 'l')
                    {
                        if (Inventory.LeftHand != null && Inventory.LeftHand.IsConsumable())
                        {
                            ApplyEffect(Inventory.LeftHand);
                            Inventory.LeftHand = null;
                        }
                        else
                        {
                            WarningMessage += "nie ma w lewej recej nic uzywalnego\n";
                        }
                    }
                    DisplayStats(room.Width);
                    break;
            }
            if(room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            _itemsToGetFromRoom = room.GetItemsAt(X, Y);
            DisplayStats(room.Width);
            WarningMessage = "";
        }

        // przyszlosciowa funkcja dla efektow
        public void ApplyEffect(IItem item)
        {
            item.ApplyEffect(this);
        }

        void PickUpItem(int n)
        {
            
            if (n >= 0 && n <_itemsToGetFromRoom.Count )
            {
                if (!_itemsToGetFromRoom[n].Equipable() )
                {
                    if (!_itemsToGetFromRoom[n].IsCurrency())
                    {
                        WarningMessage = "You can't equip this item, cause it's Unusable!\n";
                    }
                    else if (_itemsToGetFromRoom[n].GetName() == "Gold")
                    {
                        Gold += _itemsToGetFromRoom[n].GetValue();
                        _itemsToGetFromRoom.RemoveAt(n);
                    }
                    else
                    {
                        Coins += _itemsToGetFromRoom[n].GetValue();
                        _itemsToGetFromRoom.RemoveAt(n);
                    }
                }
                else
                {
                    Inventory.AddItem(_itemsToGetFromRoom[n]);
                    _itemsToGetFromRoom.RemoveAt(n);
                }
            }
        }

        public void DrinkPotion(IItem potion)
        {
            AppliedPotions.Add(potion);
        }

        public Stats GetCurrentStats()
        {
            Stats currentStats = BaseStats;
            foreach (var potion in AppliedPotions)
            {
                if (potion.IsActive(ActionCounter))
                {
                    currentStats += potion.GetBuff();
                    //WarningMessage += potion.GetName();
                }
            }
            return currentStats;
        }

        public void NextTurn()
        {
            ActionCounter++;
            AppliedPotions.RemoveAll(potion => !potion.IsActive(ActionCounter));
        }

        
        
        


        int _maxRows = 0;
        public void DisplayStats(int mapWidth)
        {
            int infoWidth = 55;
            int infoHeight = 100;
            char[,] infoGrid = new char[infoHeight, infoWidth];
            List<string> infoLines = new List<string>();

            int row = 0;
            void AddText(string text)
            {
                text = text.PadRight(infoWidth);
                text = text.Substring(0, text.Length -1) + "|";
                infoLines.Add(text);
            }
           
            var leftHand = Inventory?.LeftHand;
            var rightHand = Inventory?.RightHand;
            int attackCounter = (leftHand?.GetAttack() ?? 0) + (rightHand?.GetAttack() ?? 0);
            if (leftHand != null && leftHand.TwoHanded())
            {
                attackCounter = leftHand.GetAttack();
            }
            int luckCounter = (leftHand?.GetLuck() ?? 0) + (rightHand?.GetLuck() ?? 0);
            if(leftHand != null && leftHand.TwoHanded())
            {
                luckCounter = leftHand.GetLuck();
            }
            Stats displatyStats = GetCurrentStats();
            AddText($"Action Counter: {ActionCounter}");
            AddText("================================");
            AddText("Witchers Attributes:");
            AddText($"Power: {displatyStats.Power + attackCounter}");
            AddText($"Agility: {displatyStats.Agility}");
            AddText($"Health: {displatyStats.Health}");
            AddText($"Luck: {displatyStats.Luck + luckCounter}");
            AddText($"Aggression: {displatyStats.Attack}");
            AddText($"Wisdom: {displatyStats.Wisdom}");
            AddText("================================");
            AddText($"Coins: {Coins}");
            AddText($"Gold: {Gold}");
            AddText("================================");
            AddText($"Right Hand: {(Inventory?.RightHand != null ? Inventory.RightHand.GetName() : "None")}");
            AddText($"Left Hand: {(Inventory?.LeftHand != null ? Inventory.LeftHand.GetName() : "None")}");
            AddText("================================");
            AddText($"Number of potions Applied: {AppliedPotions.Count}");
            AddText("================================");
            AddText("Inventory:");

            if(Inventory.Items.Count == 0 || Inventory == null)
            {
                AddText("Empty");
            }
            int index = 0;
            foreach (var item in Inventory.GetItems())
            {
                AddText($"item {index}: " + item.GetName());
                index++;
            }
            AddText("================================");
            if (_itemsToGetFromRoom != null)
            {
                AddText("Items on tile:");
                for (int i = 0; i < _itemsToGetFromRoom.Count; i++)
                {
                    if(_itemsToGetFromRoom[i].IsCurrency())
                        AddText($"Item {i}: {_itemsToGetFromRoom[i].GetValue()}x {_itemsToGetFromRoom[i].GetName()}");
                    else
                        AddText($"Item {i}: {_itemsToGetFromRoom[i].GetName()}");
                }
            }
            AddText("================================");
            if (!string.IsNullOrEmpty(WarningMessage))
            {
                AddText(WarningMessage);
            }

            /*
            for (int i=infoLines.Count; i<=maxRows; i++)
            {
                AddText("");
            }
            */
            while(infoLines.Count < _maxRows)
            {
                infoLines.Add(new string(' ', infoWidth));
            }
            _maxRows = infoLines.Count;


            int cursorTop = 0;
            Console.SetCursorPosition(mapWidth + 5, cursorTop);
            foreach(var line in infoLines)
            {
                Console.SetCursorPosition(mapWidth + 5, cursorTop++);
                Console.Write(line);
            }
            //maxRows = Math.Max(maxRows, infoLines.Count);

        }

        public void DisplayAvailableKeys(int mapWidth)
        {
            int startColumn = mapWidth + 65;
            int cursorTop = 0;



            // Lista dostępnych klawiszy
            string[] keyDescriptions = new string[]
            {
                "Available keys:A",
                "[W] - Move Up",
                "[A] - Move Left",
                "[S] - Move Down",
                "[D] - Move Right",
                "[P] - Pick Up Item (then pick number from 0-9) ",
                "[R] - Equip Item in Right Hand (then pick number from 0-9)",
                "[L] - Equip Item in Left Hand (then pick numer from 0-9)",
                "[O] - Drop Item (choose hand: 'r' or 'l')",
                "[M] - Drop All Items"
            };

            // Wyświetlanie opcji
            Console.SetCursorPosition(startColumn, cursorTop);
            foreach (var description in keyDescriptions)
            {
                // Ustawienie kursora na odpowiedniej pozycji

                // Wyczyszczenie linii (opcjonalne)

                // Ponownie ustaw pozycję kursora i wyświetl opis klawisza
                Console.SetCursorPosition(startColumn, cursorTop++);
                Console.Write(description);
            }
        }

    }

}
