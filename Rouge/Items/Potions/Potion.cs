using Rouge.Items;

namespace Rouge;

internal class Potion: Item
{
    public int Duration { get; private set; }  // Jak długo trwa efekt
    public int StartActionCounter { get; private set; }  // Kiedy zaczął działać
    public Stats EffectStats { get; private set; }  // Jakie wartości zmienia

    protected Potion(string name, int duration, Stats effectStats) 
        : base(name)
    {
        Duration = duration;
        StartActionCounter = 0;
        EffectStats = effectStats;
    }

    public override void ApplyEffect(Player player)
    {
        this.StartActionCounter = player.ActionCounter;
        player.DrinkPotion(this); 
    }


    public override Stats GetBuff() => EffectStats;
    public override bool IsConsumable() => true;
    

    public override bool IsActive(int currentActionCounter)
    {
        return currentActionCounter < StartActionCounter + Duration;
    } 
}