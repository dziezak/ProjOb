namespace Rouge.ActionHandler.Handlers;

public class DropItemNumberHandler : ActionHandlerBase
{
    private DropItemHandler _previousHandler;
    public DropItemNumberHandler(DropItemHandler previousHandler)
    {
        _previousHandler = previousHandler;
    }

    public override void Handle(char input, Room room, Player player)
    {
        if(_previousHandler.IsActivated() == false)
        {
            base.Handle(input, room, player);
            UpdateUI(room, player);
            return; // wykonalem swoja prace i konicze
        }
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