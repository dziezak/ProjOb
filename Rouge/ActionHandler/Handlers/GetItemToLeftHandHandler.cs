namespace Rouge.ActionHandler.Handlers;

public class GetItemToLeftHandHandler : ActionHandlerBase
{

    public GetItemToLeftHandHandler()
    {
        _nextHandler = new GetItemToLeftHandNumberHandler(); 
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'l')
        {
            _nextHandler.Handle(input, room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }

    }
    
}