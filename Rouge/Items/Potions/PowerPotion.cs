namespace Rouge;

internal class PowerPotion : Potion
{
    public PowerPotion() : base("PowerPotion", 5, new Stats(20, 0, 0, 0, 0, 0)) { }

    public override void ApplyEffect(Player player)
    {
        player.DrinkPotion(this);
    }

    public override void Update()
    {
        Duration--;
    }
}