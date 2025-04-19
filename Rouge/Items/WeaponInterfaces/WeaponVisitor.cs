namespace Rouge.Items.WeaponInterfaces;

public class WeaponVisitor : IWeaponVisitor
{
    public static WeaponVisitor Instance { get; } = new WeaponVisitor();
    public void VisitMagic(IMagic magicWeapon, Attack attack)
    {
        Console.WriteLine($"[VISITOR] VisitorMagic. Typye: {attack.Type}");
        if (attack.Type == AttackType.Magic)
            attack.Damage = attack.BaseDamage;
        else
            attack.Damage = 1;
    }
    public void Visit(IWeapon weapon, Attack attack)
    {
        Console.WriteLine($"[VISITOR] Visiting {weapon.GetType().Name}");
        weapon.Accept(this, attack);
    }


    public void VisitLight(ILight lightWeapon, Attack attack)
    {
        Console.WriteLine("[VISITOR] VisitLight");
        if (attack.Type == AttackType.Stealth)
            attack.Damage = attack.BaseDamage * 2;
        else if (attack.Type == AttackType.Magic)
            attack.Damage = 1;
        else 
            attack.Damage = attack.BaseDamage;
    }

    public void VisitHeavy(IHeavy heavyWeapon, Attack attack)
    {
        Console.WriteLine("[VISITOR] VisitHeavy");
        if (attack.Type == AttackType.Stealth)
            attack.Damage = attack.BaseDamage / 2; // Stealth redukuje obrażenia
        else if (attack.Type == AttackType.Magic)
            attack.Damage = 1; // Atak magiczny daje minimalne obrażenia
        else
            attack.Damage = attack.BaseDamage; // Normalne obrażenia
    }

    public void VisitOther(IItem other, Attack attack)
    {
        Console.WriteLine("[VISITOR] VisitOther");
        attack.Damage = 0;
    }
}