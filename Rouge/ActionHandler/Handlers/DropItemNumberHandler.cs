namespace Rouge.ActionHandler.Handlers;

public class DropItemNumberHandler : ActionHandlerBase
{
    public DropItemNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'l' || input == 'r')
        {
            player.DropItemByHand(input, room);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}