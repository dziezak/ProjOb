namespace Rouge.Items.WeaponInterfaces;

public class WeaponVisitor : IWeaponVisitor
{
    public void VisitMagic(IMagic magicWeapon, Attack attack)
    {
        attack.Damage = attack.Type == AttackType.Magic ? attack.BaseDamage : 1;
    }

    public void VisitLight(ILight lightWeapon, Attack attack)
    {
        attack.Damage = attack.Type == AttackType.Stealth ? attack.BaseDamage * 2 : attack.BaseDamage;
    }

    public void VisitHeavy(IHeavy heavyWeapon, Attack attack)
    {
        attack.Damage = attack.Type == AttackType.Stealth ? attack.BaseDamage / 2 : attack.BaseDamage;
    }
}