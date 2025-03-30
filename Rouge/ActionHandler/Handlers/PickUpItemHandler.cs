namespace Rouge.ActionHandler.Handlers;

public class PickUpItemHandler:ActionHandlerBase
{
    private bool _isActivated = false;
    //ActionHandlerBase _alternativeHandler;
    public PickUpItemHandler()
    {
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
            else
            {
                _isActivated = true;
                player.WarningMessage = "There are some items to pick up at this position.";
                UpdateUI(room, player);
            }
        }
        else
        {
            base.Handle(input, room, player);
        }
    }

    public bool IsActivated()
    {
        return _isActivated;
    }

    public void Deactivate()
    {
        _isActivated = false;
    }

}