namespace Rouge.Items.WeaponInterfaces;

public interface IWeapon : IItem
{
    void Accept(IWeaponVisitor visitor, Attack attack);
    bool IItem.IsWeapon() => true;
}
public interface IMagic : IWeapon { }
public interface ILight : IWeapon { }
public interface IHeavy : IWeapon { }
