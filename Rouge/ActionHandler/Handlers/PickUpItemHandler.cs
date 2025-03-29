namespace Rouge.ActionHandler.Handlers;

public class PickUpItemHandler:ActionHandlerBase
{
    private bool _isActivated = false;
    public PickUpItemHandler()
    {
        _nextHandler = new PickUpItemNumberHandler(this);
    }
    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'p')
        {
            _isActivated = true;
            var items = room.GetItemsAt(player.X, player.Y);
            if (items == null || items.Count == 0)
            {
                player.WarningMessage = "No items to pick up at this position.";
                GameDisplay.Instance?.DisplayStats(room, player);
            }
            else
            {
                player.WarningMessage = "There are some items to pick up at this position.";
                GameDisplay.Instance?.DisplayStats(room, player);
            }
        }
        else
        {
            _isActivated = false;
            base.Handle(input, room, player);
        }
    }

    public bool IsActivated()
    {
        return _isActivated;
    }
}