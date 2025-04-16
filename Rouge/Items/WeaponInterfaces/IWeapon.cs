namespace Rouge.Items.WeaponInterfaces;

public interface IWeapon : IItem
{
    public int Damage { get; set; }
    public int GetMagicalDamage();
    public int GetPhysicalDamage();
    public int GetSneakyDamage();
}