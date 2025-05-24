namespace Rouge.ActionHandler;

public class EnemySelectNumberHandler :ActionHandlerBase
{

    public EnemySelectNumberHandler()
    {
    }
    public override void Handle(char enemyToAttack, Room room, Player player) 
    {
        GameDisplay.Instance?.DisplayMovementInformation("", room);
        if (player.lastCharacter != 'x')
        {
            return; // wykonalem swoja prace i konicze
        }
        if (char.IsDigit(enemyToAttack))
        {
            player.lastCharacter = enemyToAttack;
            player.Fight(room, player, enemyToAttack );
            UpdateUI(room, player);
        }
        else
        {
            player.WarningMessage += "Invalid input. Please enter a digit.\n";
            UpdateUI(room, player);
            base.Handle(enemyToAttack, room, player);
        }
    }
}