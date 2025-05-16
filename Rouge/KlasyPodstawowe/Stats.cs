namespace Rouge;

public struct Stats
{
    public int Power { get; set; }  // Siła
    public int Agility { get; set; }  // Zręczność
    public int Health { get; set; }  // Wytrzymałość
    public int Luck { get; set; }  // Szczęście
    public int Attack { get; set; }  // Atak
    public int Wisdom { get; set; }  // Mądrość

    public Stats(int power, int agility, int health, int luck, int attack, int wisdom)
    {
        Power = power;
        Agility = agility;
        Health = health;
        Luck = luck;
        Attack = attack;
        Wisdom = wisdom;
    }

    public static Stats operator +(Stats a, Stats b)
    {
        return new Stats(
            a.Power + b.Power,
            a.Agility + b.Agility,
            a.Health + b.Health,
            a.Luck + b.Luck,
            a.Attack + b.Attack,
            a.Wisdom + b.Wisdom
        );
    }
    

    public override string ToString()
    {
        return $"Power: {Power}, Agility: {Agility}, Health: {Health}, Luck: {Luck}, Attack: {Attack}, Wisdom: {Wisdom}";
    } 
}