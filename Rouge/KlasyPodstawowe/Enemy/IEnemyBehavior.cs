namespace Rouge;

public interface IEnemyBehavior
{
    void Move(Enemy enemy, Player player, Room room); // np. maze do sprawdzania możliwych ruchów
    void Act(Enemy enemy, Player player); // np. atak
}
