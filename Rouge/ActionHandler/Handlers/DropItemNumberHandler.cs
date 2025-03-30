namespace Rouge.ActionHandler.Handlers;

public class DropItemNumberHandler : ActionHandlerBase
{
    public DropItemNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if ((input == 'l' || input == 'r') && player.lastCharacter == 'o')
        {
            GameDisplay.Instance?.AddLogMessage("jest 'o' oraz jest 'l' lub 'r'");
            player.lastCharacter = input;
            player.DropItemByHand(input, room);
            UpdateUI(room, player);
        }else
        {
            GameDisplay.Instance?.AddLogMessage("Nie ma 'o'");
            UpdateUI(room, player);
            base.Handle(input, room, player);
        }
    }
}