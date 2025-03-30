namespace Rouge.ActionHandler.Handlers;

public class GetItemToLeftHandHandler : ActionHandlerBase
{

    public GetItemToLeftHandHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'l' && player.lastCharacter != 'o')
        {
            player.lastCharacter = 'l';
            //player.WarningMessage = "You are trying to get item to left hand.";
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}