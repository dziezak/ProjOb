namespace Rouge;


public class CalmBehavior : IEnemyBehavior
{
    public void Move(Enemy enemy, Player player, Room room)
    {
        // Nie porusza się
    }

    public void Act(Enemy enemy, Player player)
    {
        // Nie atakuje
    }
}

public class AggressiveBehavior : IEnemyBehavior
{
    public void Move(Enemy enemy, Player player, Room room)
    {
        // Przemieszcza się w kierunku gracza (np. BFS lub Manhattan distance)
    }

    public void Act(Enemy enemy, Player player)
    {
        // Jeśli gracz jest w zasięgu – atakuje
    }
}

public class FearfulBehavior : IEnemyBehavior
{
    public void Move(Enemy enemy, Player player, Room room)
    {
        // Ucieka od gracza (np. idzie w przeciwnym kierunku)
    }

    public void Act(Enemy enemy, Player player)
    {
        // Nie atakuje
    }
}
