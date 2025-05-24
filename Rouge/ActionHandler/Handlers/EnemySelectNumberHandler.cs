namespace Rouge.ActionHandler;

public class EnemySelectNumberHandler :ActionHandlerBase
{

    public EnemySelectNumberHandler()
    {
    }
    public override void Handle(char whichAttack, Room room, Player player) 
    {
        GameDisplay.Instance?.DisplayMovementInformation("", room);
        if (player.lastCharacter != '@')
        {
            player.lastCharacter = whichAttack;
            return; // wykonalem swoja prace i konicze
        }
        if (char.IsDigit(whichAttack) )
        {
            GameDisplay.Instance?.AddLogMessage($"Player chose attack number {whichAttack}");
            player.lastCharacter = whichAttack;
            GameDisplay.Instance?.AddLogMessage($"FUNCTION CHARACTER");
            player.Fight(room, player, whichAttack);
            UpdateUI(room, player);
        }
        else
        {
            player.WarningMessage += "Invalid input. Please enter a digit.\n";
            UpdateUI(room, player);
            base.Handle(whichAttack, room, player);
        }
    }
}