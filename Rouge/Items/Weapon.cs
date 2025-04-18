using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items
{
    public abstract class Weapon : Item, IWeapon
    {
        public int Damage { get; set; }
        public int BaseDamage { get; }

        protected Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
        }

        public abstract void Accept(IWeaponVisitor visitor, Attack attack);
        public int GetDefenseValue(Player player, AttackType type)
        {
            throw new NotImplementedException();
        }
        public override int GetAttackForBattle() => Damage;
    }
}
