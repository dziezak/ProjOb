namespace Rouge.ActionHandler.Handlers;

public class MoveLeftHandler:ActionHandlerBase
{

    public MoveLeftHandler()
    {
    }
    
    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'a')
        {
            player.MoveLeft(room);
            GameDisplay.Instance?.AddLogMessage("Player moved left using [A]");
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);   
        }
    } 
}
