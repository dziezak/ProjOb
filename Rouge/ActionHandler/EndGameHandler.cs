namespace Rouge.ActionHandler;

public class EndGameHandler : ActionHandlerBase
{

    public EndGameHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if(input == 'v')
            GameDisplay.Instance?.GameOverDisplay();
        else
            base.Handle(input, room, player);
    }
    
}