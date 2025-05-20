using System.Text.Json.Serialization;

namespace Rouge;

public struct Stats
{
    public int Power { get; set; }
    public int Agility { get; set; }
    public int Health { get; set; }
    public int Luck { get; set; }
    public int Attack { get; set; }
    public int Wisdom { get; set; } 
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