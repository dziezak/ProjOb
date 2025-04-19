using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items.DefenceVisitor;

public class DefenseVisitor : IWeaponVisitor
{
    private AttackType _attackType;
    private int _defense;

    public static DefenseVisitor Instance { get; } = new DefenseVisitor();

    public void Visit(IWeapon weapon, Attack attack)
    {
        weapon.Accept(this, attack);
    }
    public void VisitMagic(IMagic magicWeapon, Attack attack)
    {
        Console.WriteLine($"[VISITOR] VisitMagic Defense. Type: {_attackType}");

        var stats = attack.Owner.GetCurrentStats();
        if (_attackType == AttackType.Magic)
            _defense = stats.Wisdom * 2;
        else
            _defense = stats.Luck;
    }

    public void VisitLight(ILight lightWeapon, Attack attack)
    {
        Console.WriteLine("[VISITOR] VisitLight Defense");
        var stats = attack.Owner.GetCurrentStats();
        if (_attackType == AttackType.Heavy)
            _defense = stats.Agility + stats.Luck;
        else if (_attackType == AttackType.Stealth)
            _defense = stats.Agility;
        else
            _defense = stats.Luck;
    }

    public void VisitHeavy(IHeavy heavyWeapon, Attack attack)
    {
        Console.WriteLine("[VISITOR] VisitHeavy Defense");

        var stats = attack.Owner.GetCurrentStats();
        if (_attackType == AttackType.Heavy)
            _defense = stats.Power + stats.Luck;
        else if (_attackType == AttackType.Stealth)
            _defense = stats.Power;
        else
            _defense = stats.Luck;
    }

    public void VisitOther(IItem other, Attack attack)
    {
        Console.WriteLine("[VISITOR] VisitOther Defense");
        var stats = attack.Owner.GetCurrentStats();
        if (_attackType == AttackType.Heavy)
            _defense = stats.Agility;
        else if (_attackType == AttackType.Magic)
            _defense = stats.Luck;
        else
            _defense = 0;
    }

    public int GetDefense()
    {
        return _defense;
    }
}
