namespace Rouge
{
    internal interface IItem
    {
        string Name { get; }
        bool TwoHanded();
        bool Equipable();
        void ApplyEffect(Player player);
        string GetName();
        int GetAttack();
        int GetLuck();

    }
}