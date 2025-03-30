namespace Rouge.ActionHandler.Handlers;

public class MoveRightHandler:ActionHandlerBase
{

    public MoveRightHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'd')
        {
            player.lastCharacter = input;
            player.MoveRight(room);
            GameDisplay.Instance?.AddLogMessage("Player moved right using [D]");
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);   
        }
    }  
}