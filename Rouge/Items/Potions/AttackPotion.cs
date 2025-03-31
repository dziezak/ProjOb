namespace Rouge;

internal class AttackPotion : Potion
{
    public AttackPotion() : base("AggressionPotion", 5, new Stats(0, 0, 0, 0, 20, 0)) { }
    public override void ApplyEffect(Player player)
    {
        player.DrinkPotion(this);
    }

    public override void Update()
    {
        Duration--;
    }
}