namespace Rouge.ActionHandler.Handlers;

public class GetItemToRightHandHandler : ActionHandlerBase
{

    public GetItemToRightHandHandler()
    {
        _nextHandler = new GetItemToRightHandNumberHandler();
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'r')
        {
           _nextHandler.Handle(input, room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }

    }
    
}