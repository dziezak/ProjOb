namespace Rouge.ActionHandler.Handlers;

public class PickUpItemHandler:ActionHandlerBase
{
    public PickUpItemHandler()
    {
        _nextHandler = new PickUpItemNumberHandler();
    }
    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'p')
        {
            var items = room.GetItemsAt(player.X, player.Y);
            if (items == null || items.Count == 0)
            {
                player.WarningMessage = "No items to pick up at this position.";
                GameDisplay.Instance?.DisplayStats(room, player);
            }
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}