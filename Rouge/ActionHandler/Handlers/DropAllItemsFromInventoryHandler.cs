namespace Rouge.ActionHandler.Handlers;

public class DropAllItemsFromInventoryHandler:ActionHandlerBase
{
    public DropAllItemsFromInventoryHandler()
    {
    }
    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'm')
        {
            player.lastCharacter = input;
            player.DropAllItems(room);
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player); 
        }
    } 
    
}