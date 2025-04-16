namespace Rouge.Items
{
    internal class Weapon : Item
    {
        public int Damage { get; set; }

        protected Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
        }

    }
}
