namespace Rouge.Items.WeaponInterfaces;

public class WeaponVisitor : IWeaponVisitor
{
    public static WeaponVisitor Instance { get; } = new WeaponVisitor();
    public void VisitMagic(IMagic magicWeapon, Attack attack)
    {
        //Console.SetCursorPosition(50, 50);
        Console.WriteLine($"[VISITOR] VisitorMagic. Typye: {attack.Type}");
        attack.Damage = attack.Type == AttackType.Magic ? attack.BaseDamage : 1;
    }
    public void Visit(IWeapon weapon, Attack attack)
    {
        Console.WriteLine($"[VISITOR] Visiting {weapon.GetType().Name}");
        weapon.Accept(this, attack);
    }


    public void VisitLight(ILight lightWeapon, Attack attack)
    {
        //Console.SetCursorPosition(50, 50);
        Console.WriteLine("[VISITOR] VisitLight");
        attack.Damage = attack.Type == AttackType.Stealth ? attack.BaseDamage * 2 : attack.BaseDamage;
    }

    public void VisitHeavy(IHeavy heavyWeapon, Attack attack)
    {
        //Console.SetCursorPosition(50, 50);
        Console.WriteLine("[VISITOR] VisitHeavy");
        attack.Damage = attack.Type == AttackType.Stealth ? attack.BaseDamage / 2 : attack.BaseDamage;
    }

    public void VisitOther(IItem other, Attack attack)
    {
        //Console.SetCursorPosition(50, 50);
        Console.WriteLine("[VISITOR] VisitOther");
        attack.Damage = 0;
    }
}