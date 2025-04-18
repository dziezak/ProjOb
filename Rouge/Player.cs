using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Rouge.Items;
using Rouge.Items.WeaponInterfaces;

namespace Rouge
{
    public class Player
    {
        public int X {  get; set; }
        public int Y {  get; set; }
        public Inventory Inventory { get; set; }

        public Stats BaseStats { get; set; }
        public Stats AppliedStats { get; set; }
        public List<IItem> AppliedPotions = new List<IItem>();
        public int Coins {  get; set; }
        public int Gold { get; set; }

        public List<IItem> ItemsToGetFromRoom = new List<IItem>();
        public ConsoleKeyInfo _itemToPickUp;
        ConsoleKeyInfo _itemToDrop;
        int _handItem;
        public string WarningMessage = "";
        public string LogMessage = "";
        public char lastCharacter = ' ';
        public bool IsSelectingEnemies = false;
        public Enemy SelectedEnemy = null;


        public Player(int x, int y, int p, int a, int h, int l, int attack, int w, int coins, int gold)
        {
            X = x;
            Y = y;
            Inventory = new Inventory();

            // Inicjalizacja statystyk
            BaseStats = new Stats(p, a, h, l, attack, w);
            Coins = coins;
            Gold = gold;
        }
        
        public void ShowStats(Room room, Player player)
        {
            GameDisplay.Instance?.DisplayStats(room, player, false);
        }

        public void PotionFunction(char itemToUse)
        {
            if (itemToUse == 'r')
            {
                if(Inventory.RightHand != null && Inventory.RightHand.IsConsumable())
                { 
                    LogMessage += $"Witcher used {Inventory.RightHand.GetName()}.";
                    ApplyEffect(Inventory.RightHand); 
                    Inventory.RightHand = null;
                }
                else
                {
                    WarningMessage += "nie ma w prawej recej nic uzywalnego\n";
                }
            }else if(itemToUse == 'l')
            {
                if (Inventory.LeftHand != null && Inventory.LeftHand.IsConsumable())
                {
                    LogMessage += $"Witcher used {Inventory.LeftHand.GetName()}.";
                    ApplyEffect(Inventory.LeftHand);
                    Inventory.LeftHand = null;
                }
                else
                {
                    WarningMessage += "nie ma w lewej recej nic uzywalnego\n";
                }
            }
        }

        public void DrinkPotionFromInventory(char digit, Room room)
        {
            if (char.IsDigit(digit))
            {
                if (Inventory.Items.Count > int.Parse(digit.ToString()))
                {
                    IItem item = Inventory.Items[int.Parse(digit.ToString())];
                    if (item.IsConsumable())
                    {
                        Inventory.Items.Remove(item);
                        LogMessage += $"Witcher used {item.GetName()}.";
                        ApplyEffect(item); 
                    }
                    else
                    {
                        WarningMessage = "Item is not a potion";
                    }
                }
            }
            
        }
        public void DropItemFromInvetory(char digit, Room room)
        {
            if (char.IsDigit(digit))
            {
                if (Inventory.Items.Count > int.Parse(digit.ToString()))
                {
                    IItem item = Inventory.Items[int.Parse(digit.ToString())];
                    Inventory.Items.Remove(item);
                    room.DropItem(X, Y, item);
                }
            }
        }
        public void DropItemByHand(char itemToDrop, Room room)
        {
            
            if (itemToDrop == 'r')
            {
                if(Inventory.RightHand != null)
                {
                    if(Inventory.RightHand.TwoHanded())
                    {
                        Inventory.LeftHand = null;
                    }
                    LogMessage += $"Witcher dropped {Inventory.RightHand.GetName()}.";
                    room.DropItem(X, Y, Inventory.RightHand);
                    Inventory.RightHand = null;
                }
                else
                {
                    WarningMessage += "Nie trzymasz nic w prawej rece\n";
                }
            }else if(itemToDrop == 'l')
            {
                if (Inventory.LeftHand != null)
                {
                    if(Inventory.LeftHand.TwoHanded())
                    {
                        Inventory.RightHand = null;
                    }
                    LogMessage += $"Witcher dropped {Inventory.LeftHand.GetName()}.";
                    room.DropItem(X, Y, Inventory.LeftHand);
                    Inventory.LeftHand = null;
                }
                else
                {
                    WarningMessage += "Nie trzymasz nic w lewej rece\n";
                }
            }
        }

        public void LeftHand(char itemToPickUp, Room room)
        {
            if (char.IsDigit(itemToPickUp))
            {
                _handItem = int.Parse(itemToPickUp.ToString());
                Inventory.EquipItemLeftHand(_handItem, this);
            }
        }
        public void RightHand(char itemToPickUp, Room room)
        {
            if(char.IsDigit(itemToPickUp))
            {
                _handItem = int.Parse(itemToPickUp.ToString());
                Inventory.EquipItemRightHand(_handItem, this);
            }
        }
        
        public void DropAllItems(Room room)
        {
            var itemsToRemove = Inventory.GetItems().ToList();
            LogMessage += $"Witcher dropped all his inventory.";
            foreach (var item in itemsToRemove)
            {
                room.DropItem(X, Y, item);
                Inventory.RemoveItem(item);
            }
        }

        public void MoveRight(Room room)
        {
            int newX = X, newY = Y;
            newX++;
            if (room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
                Timer.NextTurn(this);
            }
        }

        public void MoveLeft(Room room)
        {
            int newX = X, newY = Y;
            newX--;
            if(room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
                Timer.NextTurn(this);
            }
        }

        public void MoveDown(Room room)
        {
            int newX = X, newY = Y;
            newY++;
            if(room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
                Timer.NextTurn(this);
            }
        }

        public void MoveUp(Room room)
        {
            int newX = X, newY = Y;
            newY--;
            if(room.IsWalkable(newX, newY))
            {
                X = newX;
                Y = newY;
                Timer.NextTurn(this);
            }
        }

        public void ApplyEffect(IItem item)
        {
            item.ApplyEffect(this);
        }

        public void PickUpItem(int n)
        {
            if (n >= 0 && n <ItemsToGetFromRoom.Count )
            {
                if (!ItemsToGetFromRoom[n].Equipable() )
                {
                    if (!ItemsToGetFromRoom[n].IsCurrency())
                    {
                        WarningMessage = "You can't equip this item, cause it's Unusable!\n";
                    }
                    else if (ItemsToGetFromRoom[n].GetName() == "Gold")
                    {
                        Gold += ItemsToGetFromRoom[n].GetValue();
                        LogMessage += $"Witcher picked up Gold.";
                        ItemsToGetFromRoom.RemoveAt(n);
                    }
                    else
                    {
                        Coins += ItemsToGetFromRoom[n].GetValue();
                        LogMessage += $"Witcher picked up Coins.";
                        ItemsToGetFromRoom.RemoveAt(n);
                    }
                }
                else
                {
                    LogMessage += $"Witcher picked up {ItemsToGetFromRoom[n].GetName()}.";
                    Inventory.AddItem(ItemsToGetFromRoom[n]);
                    ItemsToGetFromRoom.RemoveAt(n);
                }
            }
        }

        public void DrinkPotion(IItem potion)
        {
            AppliedPotions.Add(potion);
            potion.Subscribe(this);
        }

        public Stats GetCurrentStats()
        {
            Stats currentStats = BaseStats;
            foreach (var potion in AppliedPotions)
            {
                if (potion.IsActive(Timer.GetActionCounter()))
                {
                    currentStats += potion.GetBuff();
                }
            }
            return currentStats;
        }


        public void RemovePotion(IItem potion)
        {
            AppliedPotions.Remove(potion);
            potion.Unsubscribe(this);
        }

        public void FlushPotions()
        {
            AppliedPotions.Clear();
        }

        public int CurrentHealh;
        public int CurrentEnemyHealth;

        public void Fight()
        {
            Console.Clear();
            Console.SetCursorPosition(0, Console.CursorTop);
            var playerStats = this.GetCurrentStats();
            CurrentHealh = playerStats.Health;
            CurrentEnemyHealth = SelectedEnemy.EnemyStats.Health;
            
            GameDisplay.Instance?.RenderBattleUI(this);
            GameDisplay.Instance?.RenderHeathBar(CurrentHealh, playerStats.Health, "witcher", true);
            //GameDisplay.Instance?.DisplayAvailableString(SelectedEnemy.GetImage(), 0);
            GameDisplay.Instance?.RenderHeathBar(CurrentEnemyHealth, SelectedEnemy.EnemyStats.Health,SelectedEnemy.GetName(), false);
            //Dispaly Available Attacks
            //GameDisplay.Instance?.DisplayLog(16, 70);
            DisplayAvailableAttacks();
            
            while (CurrentHealh > 0 && CurrentEnemyHealth > 0)
            {
                char input = Console.ReadKey(true).KeyChar;
                AttackType attackType = GetAttackType(input);

                int baseLeftDamage = this.Inventory.LeftHand?.GetAttack() ?? 0;
                int baseRightDamage = this.Inventory.RightHand?.GetAttack() ?? 0;
                
                Attack attackRight = new Attack(attackType, baseLeftDamage);
                Attack attackLeft = new Attack(attackType, baseRightDamage);
                
                if (this.Inventory.RightHand is IWeapon rightWeapon)
                {
                    attackRight.Apply(rightWeapon);
                }
                else
                {
                    attackRight.Damage = 0;
                }

                if (this.Inventory.LeftHand is IWeapon leftWeapon)
                {
                    attackLeft.Apply(leftWeapon);
                }
                else
                {
                    attackLeft.Damage = 0;
                }
                
                int totalDamage = attackRight.Damage + attackLeft.Damage;
                CurrentEnemyHealth -= totalDamage;
                
                GameDisplay.Instance?.RenderHeathBar(CurrentEnemyHealth, SelectedEnemy.EnemyStats.Health, SelectedEnemy.GetName(), false);
                
                //GameDisplay.Instance?.AddLogMessage($"Player attacked with {attackType}, dealing {totalDamage} damage!");
                if (CurrentEnemyHealth <= 0)
                {
                    //GameDisplay.Instance?.AddLogMessage($"{SelectedEnemy.GetName()} is defeated!");
                    break;
                }

                int enemyAttackDamage = SelectedEnemy.EnemyStats.Attack;
                CurrentHealh -= enemyAttackDamage;
                GameDisplay.Instance?.RenderHeathBar(CurrentHealh, playerStats.Health, "witcher", true);
                

                if (CurrentHealh <= 0)
                {
                    Console.ReadKey(); // przerwa aby zobaczyc, ze przegralismy ( HeathBar)
                    Game.isGameOver = true;
                }
            }
            
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Clear();
        }
        
        private AttackType GetAttackType(char input)
        {
            return input switch
            {
                '1' => AttackType.Heavy,
                '2' => AttackType.Stealth,
                '3' => AttackType.Magic,
                _ => AttackType.Heavy
            };
        }
        
        private void DisplayAvailableAttacks()
        {
            Console.SetCursorPosition(30, 30);
            Console.WriteLine("\nAvailable Attacks:");

            string format = "{0,-15} | Damage: {1,3}";
            var leftHandWeapon = (IWeapon) this.Inventory?.LeftHand;
            var rightHandWeapon = (IWeapon) this.Inventory?.RightHand;

            int leftBase = leftHandWeapon?.BaseDamage ?? 0;
            int rightBase = rightHandWeapon?.BaseDamage ?? 0;

            // Tworzymy obiekty ataku, aby Visitor poprawnie obliczył rzeczywiste obrażenia
            Attack normalLeft = new Attack(AttackType.Heavy, leftBase);
            Attack normalRight = new Attack(AttackType.Heavy, rightBase);
            Attack stealthLeft = new Attack(AttackType.Stealth, leftBase);
            Attack stealthRight = new Attack(AttackType.Stealth, rightBase);
            Attack magicLeft = new Attack(AttackType.Magic, leftBase);
            Attack magicRight = new Attack(AttackType.Magic, rightBase);

            // Zastosowanie Visitor dla każdej broni

            if (leftHandWeapon != null)
            {
                normalLeft.Apply(leftHandWeapon);
                stealthLeft.Apply(leftHandWeapon);
                magicLeft.Apply(leftHandWeapon);
            }

            if (rightHandWeapon != null)
            {
                normalRight.Apply(rightHandWeapon);
                stealthRight.Apply(rightHandWeapon);
                magicRight.Apply(rightHandWeapon);
            }

            // Wyświetlenie danych
            Console.WriteLine(format, "1 - Normal", normalLeft.Damage + normalRight.Damage);
            Console.WriteLine(format, "2 - Stealth", stealthLeft.Damage + stealthRight.Damage);
            Console.WriteLine(format, "3 - Magic", magicLeft.Damage + magicRight.Damage);
        }


    }

}
