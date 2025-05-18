using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items
{
    public abstract class Weapon : Item, IWeapon
    {
        public int Damage { get; set; }

        protected Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
            typeToSerialize = "Weapon";
        }

        public abstract void Accept(IWeaponVisitor visitor, Attack attack);
        public int GetDefenseValue(Player player, AttackType type)
        {
            throw new NotImplementedException();
        }
        public override int GetAttack() => Damage;
    }
}
