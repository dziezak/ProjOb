namespace Rouge.ActionHandler.Handlers;

public class DropItemHandHandler : ActionHandlerBase
{
    public DropItemHandHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if ((input == 'l' || input == 'r') && player.lastCharacter == 'o')
        {
            //GameDisplay.Instance?.AddLogMessage("jest 'o' oraz jest 'l' lub 'r'");
            player.lastCharacter = input;
            player.DropItemByHand(input, room);
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}