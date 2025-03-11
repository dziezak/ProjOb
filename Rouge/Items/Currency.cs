namespace Rouge.Items
{
    internal class Currency : Item
    {
        public int Value { get; set; }

        public Currency(string name, int value) : base(name)
        {
            Value = value;
        }
        public override bool IsCurrency() => true;
        public override bool Equipable() => false;
        public override int GetValue() => Value;
    }
    class Gold : Currency
    {
        public Gold(int value) : base("Gold", value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} (Value: {Value}";
        }
    }

    class Coin : Currency
    {
        public Coin(int value) : base("Coin", value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} (Value: {Value})";
        }
    }
}
