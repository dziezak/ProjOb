namespace Rouge.ActionHandler.Handlers;

public class DropItemHandler:ActionHandlerBase
{
    private bool _isActivated = false;
    public DropItemHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'o')
        {
            _isActivated = true;
            player.WarningMessage = "You are trying to drop it.";
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