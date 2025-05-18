namespace Rouge;

internal class AttackPotion : Potion
{
    public AttackPotion() : base("AggressionPotion", 5, new Stats(0, 0, 0, 0, 20, 0))
    {
        typeToSerialize = "AttackPotion";
    }
    public override void ApplyEffect(Player player)
    {
        player.DrinkPotion(this);
    }

    public override void Update(Player player)
    {
        if (Duration > 1)
        {
            Duration--;
        }
        else
        {
            Unsubscribe(player);    
            player.RemovePotion(this);
        }
    }
}