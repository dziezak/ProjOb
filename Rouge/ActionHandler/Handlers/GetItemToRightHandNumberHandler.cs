namespace Rouge.ActionHandler.Handlers;

public class GetItemToRightHandNumberHandler : ActionHandlerBase
{
    public GetItemToRightHandNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (player.lastCharacter == 'r' && char.IsDigit(input))
        {
            player.lastCharacter = input;
            player.RightHand(input, room);
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}