using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items;

public enum AttackType { Heavy, Stealth, Magic }
public class Attack
{
    public AttackType Type { get; }
    public int BaseDamage { get; }
    public int Damage { get; set; }

    public Attack(AttackType type, int baseDamage)
    {
        Type = type;
        BaseDamage = baseDamage;
        Damage = 0;
    }

    public void Apply(IWeapon weapon)
    {
        Console.WriteLine($"[ATTACK] Apply() called for {weapon.GetType().Name}, base dmg: {BaseDamage}");
        WeaponVisitor.Instance.Visit(weapon, this);
    }
}