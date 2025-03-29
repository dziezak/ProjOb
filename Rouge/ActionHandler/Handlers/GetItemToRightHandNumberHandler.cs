namespace Rouge.ActionHandler.Handlers;

public class GetItemToRightHandNumberHandler : ActionHandlerBase
{
    public GetItemToRightHandNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (char.IsDigit(input))
        {
            player.RightHand(input, room);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}