namespace Rouge.ActionHandler.Handlers;

public class GetItemToLeftHandNumberHandler : ActionHandlerBase
{
    private readonly GetItemToLeftHandHandler _previousHandler;
    public GetItemToLeftHandNumberHandler(GetItemToLeftHandHandler getItemToLeftHandHandler)
    {
       _previousHandler = getItemToLeftHandHandler; 
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (_previousHandler.IsActivated() == false)
        {
            base.Handle(input, room, player);
            UpdateUI(room, player);
            return;
        }
        if (char.IsDigit(input))
        {
            _previousHandler.Deactivate(); 
            player.LeftHand(input, room);
        }
        else
        {
            _previousHandler.Deactivate();
            base.Handle(input, room, player);
        }
    }
}