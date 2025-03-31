namespace Rouge;

internal class WisdomPotion : Potion
{
    public WisdomPotion() : base("WisdomPotion", 5, new Stats(0, 0, 0, 0, 0, 20)) { }

    public override void ApplyEffect(Player player)
    {
        player.DrinkPotion(this);
    }

    public override void Update()
    {
       //nothing to do 
    }
}