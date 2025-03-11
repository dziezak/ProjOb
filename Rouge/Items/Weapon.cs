namespace Rouge.Items
{
    internal class Weapon : Item
    {
        public int Damage { get; set; }

        protected Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
        }

        public override void ApplyEffect(Player player)
        {
            //player.DisplayStats();
        }

        public override int GetAttack() => 0;
        public override int GetLuck() => 0; 
    }
}
