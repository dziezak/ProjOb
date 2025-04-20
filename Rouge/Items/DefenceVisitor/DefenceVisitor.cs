using System.Reflection.Metadata;
using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items.DefenceVisitor;

public class DefenseVisitor : IWeaponVisitor
{

    public static DefenseVisitor Instance { get; } = new DefenseVisitor();


    private DefenseVisitor()
    {
    }

    public void Visit(IWeapon weapon, Attack attack)
    {
        weapon.Accept(this, attack);
    }
    public void VisitMagic(IMagic magicWeapon, Attack attack)
    {
        Console.WriteLine($"[VISITOR] VisitMagic Defense. Type: {attack.Type}");

        var stats = attack.Owner.GetCurrentStats();
        if (attack.Type == AttackType.Magic)
            attack.Defense = stats.Wisdom * 2;
        else
            attack.Defense = stats.Luck;
    }

    public void VisitLight(ILight lightWeapon, Attack attack)
    {
        //Console.WriteLine("[VISITOR] VisitLight Defense");
        var stats = attack.Owner.GetCurrentStats();
        if (attack.Type == AttackType.Heavy)
            attack.Defense = stats.Agility + stats.Luck;
        else if (attack.Type == AttackType.Stealth)
            attack.Defense = stats.Agility;
        else
            attack.Defense = stats.Luck;
    }

    public void VisitHeavy(IHeavy heavyWeapon, Attack attack)
    {
        //Console.WriteLine("[VISITOR] VisitHeavy Defense");

        var stats = attack.Owner.GetCurrentStats();
        if (attack.Type == AttackType.Heavy)
            attack.Defense = stats.Power + stats.Luck;
        else if (attack.Type == AttackType.Stealth)
            attack.Defense = stats.Power;
        else
            attack.Defense = stats.Luck;
    }

    public void VisitOther(IItem other, Attack attack)
    {
        //Console.WriteLine("[VISITOR] VisitOther Defense");
        var stats = attack.Owner.GetCurrentStats();
        if (attack.Type == AttackType.Heavy)
            attack.Defense = stats.Agility;
        else if (attack.Type == AttackType.Magic)
            attack.Defense = stats.Luck;
        else
            attack.Defense = 0;
    }

}
