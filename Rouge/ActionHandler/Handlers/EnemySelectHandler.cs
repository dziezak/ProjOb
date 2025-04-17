namespace Rouge.ActionHandler.Handlers;

public class EnemySelectHandler: ActionHandlerBase
{
    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'x')
        {
            player.lastCharacter = 'x';
            var enemiesNearBy = room.GetEnemiesNearBy(player.Y, player.X);
            if (enemiesNearBy.Count > 0)
            {
                //GameDisplay.Instance?.DisplayMovementInformation("pick number of item to pick up", room);
                GameDisplay.Instance?.DisplayMovementInformation($"Enemies detected! Choose one to fight.", room);
                player.IsSelectingEnemies = true;
            }
            else
            {
                GameDisplay.Instance?.AddLogMessage("No enemies nearby");
            }
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
    
}