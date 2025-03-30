namespace Rouge.ActionHandler.Handlers;

public class MoveUpHandler: ActionHandlerBase
{

    public MoveUpHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'w')
        {
            player.lastCharacter = input;
            player.MoveUp(room);
            GameDisplay.Instance?.AddLogMessage("Player moved up using [W]");
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player); 
        }
    }
}