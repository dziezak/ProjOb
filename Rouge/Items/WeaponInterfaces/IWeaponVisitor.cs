namespace Rouge.Items.WeaponInterfaces;

public interface IWeaponVisitor
{
    void VisitMagic(IMagic magicWeapon, Attack attack);
    void VisitLight(ILight lightWeapon, Attack attack);
    void VisitHeavy(IHeavy heavyWeapon, Attack attack);
} 