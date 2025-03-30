namespace Rouge.ActionHandler.Handlers;

public class GetItemToRightHandHandler : ActionHandlerBase
{

    public GetItemToRightHandHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'r' && player.lastCharacter != 'o')
        {
            player.lastCharacter = 'r';
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }

    }
    
}