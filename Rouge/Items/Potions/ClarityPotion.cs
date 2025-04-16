namespace Rouge;

internal class ClarityPotion : Potion
{
    public ClarityPotion() : base("ClarityPotion", 1, new Stats(0, 0, 0, 0, 0, 0)) { }

    public override void ApplyEffect(Player player)
    {
        player.FlushPotions();
        Timer.ClearPotions(player);
    }

    public override void Update(Player player)
    {
        Duration--;
    }
}