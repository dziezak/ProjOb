using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Rouge.Items;
using Rouge.Items.WeaponInterfaces;

namespace Rouge
{
    public class Player
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Inventory Inventory { get; set; }
        public Stats BaseStats { get; set; }
        public List<IItem> AppliedPotions { get; set; } = new List<IItem>();
        public int Coins { get; set; }
        public int Gold { get; set; }
        public List<IItem> ItemsToGetFromRoom { get; set; } = new List<IItem>();
        public string WarningMessage { get; set; } = "";
        public string LogMessage { get; set; } = "";
        public char lastCharacter { get; set; } = ' ';
        public bool IsSelectingEnemies { get; set; } = false;
        public Enemy SelectedEnemy { get; set; } = null;
        public ConsoleKeyInfo _itemToPickUp;
        ConsoleKeyInfo _itemToDrop;
        int _handItem;
        
        //Things for Fight:
        public bool IsFighting { get; set; } = false;
        public int CurrentHealh { get; set; } = 0;
        
        public string AvailableAttacks { get; set; } = "";
        

        public Player(int id, int x, int y, Inventory inventory, Stats baseStats, List<IItem> appliedPotions, 
            int coins, int gold, List<IItem> itemsToGetFromRoom, bool isFighting, int currentHealh, Enemy selectedEnemy, 
            string availableAttacks)
        {
            Id = id;
            X = x;
            Y = y;
            Inventory = inventory ?? new Inventory();
            BaseStats = baseStats;
            AppliedPotions = appliedPotions ?? new List<IItem>();
            ItemsToGetFromRoom = itemsToGetFromRoom ?? new List<IItem>();
            Coins = coins;
            Gold = gold;
            IsFighting = isFighting;
            CurrentHealh = currentHealh;
            SelectedEnemy = selectedEnemy;
            AvailableAttacks = availableAttacks;
        }
 

        public Player()
        {
            Inventory = new Inventory();
            BaseStats = new Stats();
            AppliedPotions = new List<IItem>();
            ItemsToGetFromRoom = new List<IItem>();
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


        public void Fight(Room room, Player player, char input) 
        {
            if(player.IsFighting == false){
                player.CurrentHealh = player.GetCurrentStats().Health;
                player.IsFighting = true;
            }

            //room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)];
            
            
            /*
            GameDisplay.Instance?.RenderBattleUI(player);
            GameDisplay.Instance?.RenderHeathBar(CurrentHealh, playerStats.Health, "witcher", true);
            GameDisplay.Instance?.DisplayAvailableString(player.SelectedEnemy.GetImage(), 0);
            GameDisplay.Instance?.RenderHeathBar(CurrentEnemyHealth,player.SelectedEnemy.EnemyStats.Health ,player.SelectedEnemy.GetName(), false);
            GameDisplay.Instance?.DisplayLog(0, 70);
            */
            
            if (player.CurrentHealh > 0 && room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].CurrentHealth > 0)
            {
                /*
                GameDisplay.Instance?.DisplayAvailableAttacks(player);
                GameDisplay.Instance?.DisplayLog(0, 70);
                */
                player.AvailableAttacks = GetAvailableAttacks(player);
                AttackType attackType = GetAttackType(input);

                int baseLeftDamage = player.Inventory.LeftHand?.GetAttack() ?? 0;
                int baseRightDamage = player.Inventory.RightHand?.GetAttack() ?? 0;
                
                Attack attackLeft = new Attack(attackType, baseLeftDamage, player);
                Attack attackRight = new Attack(attackType, baseRightDamage, player);
                
                if (player.Inventory.RightHand != null)
                {
                    attackRight.Apply((IWeapon)player.Inventory.RightHand);
                }

                if (player.Inventory.LeftHand != null)
                {
                    attackLeft.Apply((IWeapon)player.Inventory.LeftHand);
                }
                
                int totalDamage = attackRight.Damage + attackLeft.Damage;
                room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].CurrentHealth -= totalDamage;
                if (room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].CurrentHealth /
                    room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].EnemyStats.Health <= 0.2)
                {
                    room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].Behavior = new FearfulBehavior();
                    GameDisplay.Instance?.AddLogMessage(
                        $"{room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].Name} is running away");
                }
                
                GameDisplay.Instance?.AddLogMessage($"Player attacked with {attackType}, dealing {totalDamage} damage!");
                //GameDisplay.Instance?.RenderHeathBar(CurrentEnemyHealth, player.SelectedEnemy.EnemyStats.Health, player.SelectedEnemy.GetName(), false);
                if (room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].CurrentHealth <= 0)
                {
                    var key = (room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].Y, room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].X);
                    if (room._enemiesMap.ContainsKey(key))
                    {
                        room._enemiesMap.Remove(key);
                    }
                    else
                    {
                        GameDisplay.Instance?.AddLogMessage($"There is no enemy on {room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].Y}, {room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].X}");
                        WarningMessage = "There is no enemy on {player.SelectedEnemy.Y}, {player.SelectedEnemy.X}";
                    }
                    GameDisplay.Instance?.AddLogMessage($"{room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].GetName()} is defeated!");
                    IsFighting = false;
                }

                int playerDefense = attackLeft.Defense + attackRight.Defense;
                int enemyAttackDamage = Math.Max(0, room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].EnemyStats.Power - playerDefense);
                GameDisplay.Instance?.AddLogMessage($"Player got attacked {room._enemiesMap[(player.SelectedEnemy.Y, player.SelectedEnemy.X)].EnemyStats.Power}, but blocked {playerDefense} damage!");
                CurrentHealh -= enemyAttackDamage;
                //GameDisplay.Instance?.RenderHeathBar(CurrentHealh, playerStats.Health, "witcher", true);
                if (CurrentHealh <= 0)
                {
                    //GameDisplay.Instance?.GameOverDisplay();
                    IsFighting = false;
                }
            }
            //Console.SetCursorPosition(0, Console.CursorTop);
            //Console.Clear();
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
        
        public string GetAvailableAttacks(Player player)
        {
            if (player == null || player.Inventory == null)
                return "No available attacks.";

            StringBuilder ret = new StringBuilder("\nAvailable Attacks:\n");

            string format = "{0,-15} | Damage: {1,3} | Defense: {2,3}\n";
    
            int leftBase = player.Inventory.LeftHand?.GetAttack() ?? 0;
            int rightBase = player.Inventory.RightHand?.GetAttack() ?? 0;

            Attack normalLeft = new Attack(AttackType.Heavy, leftBase, player);
            Attack normalRight = new Attack(AttackType.Heavy, rightBase, player);
            Attack stealthLeft = new Attack(AttackType.Stealth, leftBase, player);
            Attack stealthRight = new Attack(AttackType.Stealth, rightBase, player);
            Attack magicLeft = new Attack(AttackType.Magic, leftBase, player);
            Attack magicRight = new Attack(AttackType.Magic, rightBase, player);

            if (player.Inventory.LeftHand is IWeapon leftWeapon)
            {
                normalLeft.Apply(leftWeapon);
                stealthLeft.Apply(leftWeapon);
                magicLeft.Apply(leftWeapon);
            }

            if (player.Inventory.RightHand is IWeapon rightWeapon)
            {
                normalRight.Apply(rightWeapon);
                stealthRight.Apply(rightWeapon);
                magicRight.Apply(rightWeapon);
            }

            ret.AppendFormat(format, "1 - Normal", normalLeft.Damage + normalRight.Damage, normalLeft.Defense + normalRight.Defense);
            ret.AppendFormat(format, "2 - Stealth", stealthLeft.Damage + stealthRight.Damage, stealthLeft.Defense + stealthRight.Defense);
            ret.AppendFormat(format, "3 - Magic", magicLeft.Damage + magicRight.Damage, magicLeft.Defense + magicRight.Defense);

            return ret.ToString();
        }
 

    }

}
