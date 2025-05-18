namespace Rouge.ActionHandler;

public class ChooseEnemyHandler : ActionHandlerBase
{
    public override void Handle(char input, Room room, Player player)
    {
        if (player.IsSelectingEnemies && char.IsDigit(input))
        {
            var enemiesNearBy = room.GetEnemiesNearBy(player.Y, player.X);
            int enemyIndex = int.Parse(input.ToString());

            if (enemyIndex >= 0 & enemyIndex < enemiesNearBy.Count)
            {
                player.lastCharacter = input;
                var selectedEnemy = enemiesNearBy[enemyIndex];
                GameDisplay.Instance?.AddLogMessage($"Witcher decided to fight {selectedEnemy.GetName()}");
                player.SelectedEnemy = selectedEnemy;
                player.IsSelectingEnemies = false;
                //TODO: do napisania poprawnie
                //player.Fight(room); 
            }
            else
            {
                GameDisplay.Instance?.AddLogMessage("Invalid enemy selection");
            }
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
    
}