namespace Rouge.ActionHandler.Handlers;

public class GetItemToRightHandHandler : ActionHandlerBase
{

    public GetItemToRightHandHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'r' && (player.lastCharacter != 'o' && player.lastCharacter != 'e'))
        {
            GameDisplay.Instance?.DisplayMovementInformation("Pick number of item to pick up", room);
            player.lastCharacter = 'r';
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }

    }
    
}