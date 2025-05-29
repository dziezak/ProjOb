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
        int dx = player.X - enemy.X;
        int dy = player.Y - enemy.Y;

        int newX = enemy.X;
        int newY = enemy.Y;

        if (dx != 0)
            newX += Math.Sign(dx);
        else if (dy != 0)
            newY += Math.Sign(dy);

        if (room.IsWalkable(newX, newY))
        {
            if(!(newX == player.X && newY == player.Y))
                room.MoveEnemy(enemy, newX, newY);
        }
    }

    public void Act(Enemy enemy, Player player)
    {
        int dist = Math.Abs(enemy.X - player.X) + Math.Abs(enemy.Y - player.Y);

        if (dist == 1)
        {
            GameDisplay.Instance?.AddLogMessage($"{enemy.Name} jumped attacked Player {player.Id}!");
            player.IsFighting = true;
            player.CurrentHealh -= enemy.EnemyStats.Power; // przykładowo
        }
    }
}


public class FearfulBehavior : IEnemyBehavior
{
    public void Move(Enemy enemy, Player player, Room room)
    {
        var directions = new (int dx, int dy)[]
        {
            (0, -1), 
            (0, 1),  
            (-1, 0), 
            (1, 0), 
        };

        int maxDist = Manhattan(enemy.X, enemy.Y, player.X, player.Y);
        int bestX = enemy.X;
        int bestY = enemy.Y;

        foreach (var (dx, dy) in directions)
        {
            int newX = enemy.X + dx;
            int newY = enemy.Y + dy;

            if (room.IsWalkable(newX, newY))
            {
                int dist = Manhattan(newX, newY, player.X, player.Y);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    bestX = newX;
                    bestY = newY;
                }
            }
        }

        room.MoveEnemy(enemy, bestX, bestY);
    }

    public void Act(Enemy enemy, Player player)
    {
        // Nie atakuje
    }

    private int Manhattan(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }
}

