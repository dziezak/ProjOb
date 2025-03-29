namespace Rouge.ActionHandler.Handlers;

public class DropAllItemsFromInventoryHandler:ActionHandlerBase
{
    public DropAllItemsFromInventoryHandler(ActionHandlerBase nextHandler)
    {
        _nextHandler = nextHandler;
    }
    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'm')
        {
            player.DropAllItems(room);
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player); 
        }
    } 
    
}