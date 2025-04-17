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
    }

    public void Apply(IWeapon? weapon)
    {
        if (weapon == null) return;
        WeaponVisitor visitor = new WeaponVisitor();
        weapon.Accept(visitor, this);
    }
    
}