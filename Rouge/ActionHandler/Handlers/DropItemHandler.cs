namespace Rouge.ActionHandler.Handlers;

public class DropItemHandler:ActionHandlerBase
{
    public DropItemHandler()
    {
        _nextHandler = new DropItemNumberHandler();
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'o')
        {
            _nextHandler.Handle(input, room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }

    } 
    
}