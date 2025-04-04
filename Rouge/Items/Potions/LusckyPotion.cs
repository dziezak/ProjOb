namespace Rouge;

internal class LuckyPotion : Potion
{
    public LuckyPotion() : base("LuckyPotion", 5, new Stats(0, 0, 0, 2, 0, 0)) { }

    public override void ApplyEffect(Player player)
    {
        player.DrinkPotion(this);
    }

    public override void Update(Player player)
    {
        if (Duration > 0)
        {
            EffectStats = EffectStats with { Luck = EffectStats.Luck * (Duration) };
            Duration--;
        }
        else
        {
            Unsubscribe(player);    
            player.RemovePotion(this);
        }
    }
}