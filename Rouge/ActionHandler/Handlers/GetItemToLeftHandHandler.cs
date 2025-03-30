namespace Rouge.ActionHandler.Handlers;

public class GetItemToLeftHandHandler : ActionHandlerBase
{

    private bool _isActivated = false;
    public GetItemToLeftHandHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'l')
        {
            _isActivated = true;
            player.WarningMessage = "You are trying to get item to left hand.";
            UpdateUI(room, player);
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