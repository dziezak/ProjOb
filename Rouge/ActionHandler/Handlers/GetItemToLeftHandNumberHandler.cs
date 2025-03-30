namespace Rouge.ActionHandler.Handlers;

public class GetItemToLeftHandNumberHandler : ActionHandlerBase
{
    public GetItemToLeftHandNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        
        if (player.lastCharacter == 'l' &&  char.IsDigit(input))
        {
            player.lastCharacter = input;
            player.LeftHand(input, room);
            UpdateUI(room, player);
        }else
        {
            base.Handle(input, room, player);
        }
    }
}