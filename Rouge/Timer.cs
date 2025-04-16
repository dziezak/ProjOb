using System.Runtime.CompilerServices;
using Rouge.Items;

namespace Rouge;

public static class Timer
{
    private static int ActionCounter { get; set; } = 0;
    private static List<IItem> _activePotions { get; set; } = new List<IItem>();
    //public static event Action? OnNextTurn;
    
    public static void NextTurn(Player player)
    {
        ActionCounter++;

        foreach (Potion potion in _activePotions.ToList())
        {
            potion.Update(player);

            if (!potion.IsActive(ActionCounter))
            {
                potion.Unsubscribe(player);
                player.RemovePotion(potion);
            }
        }
    }

    public static void ClearPotions(Player player)
    {
        _activePotions.Clear();
    }
    public static void AddPotion(IItem potion)
    {
        _activePotions.Add(potion);
    }

    public static void RemovePotion(IItem potion)
    {
        _activePotions.Remove(potion);
        Console.WriteLine($"{potion.Name} effect removed.");
    }

    public static int GetActionCounter()
    {
        return ActionCounter;
    }
}