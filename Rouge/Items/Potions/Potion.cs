using Rouge.Items;

namespace Rouge;

internal class Potion: Item
{
    public int Duration { get; set; }  // Jak długo trwa efekt
    public Stats EffectStats { get; set; }  // Jakie wartości zmienia

    protected Potion(string name, int duration, Stats effectStats) 
        : base(name)
    {
        Duration = duration;
        EffectStats = effectStats;
    }

    public override Stats GetBuff() => EffectStats;
    public override bool IsConsumable() => true;


    public override bool IsActive(int currentActionCounter)
    {
        return Duration > 0;
    } 
}