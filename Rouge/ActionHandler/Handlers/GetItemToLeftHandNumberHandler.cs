namespace Rouge.ActionHandler.Handlers;

public class GetItemToLeftHandNumberHandler : ActionHandlerBase
{
    public GetItemToLeftHandNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (char.IsDigit(input))
        {
            player.LeftHand(input, room);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}