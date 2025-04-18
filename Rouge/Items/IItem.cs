using Rouge.Items.WeaponInterfaces;

namespace Rouge.Items
{
    public interface IItem
    {
        string Name { get; }
        bool TwoHanded();
        bool Equipable();
        void ApplyEffect(Player player);
        string GetName();
        int GetAttack();
        int GetLuck();
        bool IsCurrency();
        int GetValue();
        Stats GetBuff();
        public bool IsActive(int currentActionCounter);
        bool IsConsumable();

        bool IsWeapon();
        //do potionow
        public void Update(Player player);
        public void Subscribe(Player player);
        public void Unsubscribe(Player player);
        
        // do Visitora:
    }
}