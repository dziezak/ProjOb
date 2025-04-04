namespace Rouge;

public static class Timer
{
    private static int ActionCounter { get; set; } = 0;
    public static event Action? OnNextTurn;

    public static void NextTurn()
    {
       ActionCounter++; 
       OnNextTurn?.Invoke();
    }
}