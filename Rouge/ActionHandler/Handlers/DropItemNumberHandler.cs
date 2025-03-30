namespace Rouge.ActionHandler.Handlers;

public class DropItemNumberHandler : ActionHandlerBase
{
    public DropItemNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (char.IsDigit(input) && player.lastCharacter == 'o')
        {
            //GameDisplay.Instance?.AddLogMessage("Nowy handler wyrzucenia z inventory");
            GameDisplay.Instance?.DisplayMovementInformation("", room);
            player.lastCharacter = input;
            player.DropItemFromInvetory(input, room);
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    } 
}