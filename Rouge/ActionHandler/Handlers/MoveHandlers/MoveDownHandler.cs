namespace Rouge.ActionHandler.Handlers;

public class MoveDownHandler:ActionHandlerBase
{

    public MoveDownHandler() { }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 's')
        {
            player.lastCharacter = 's';
            player.MoveDown(room);
            GameDisplay.Instance?.AddLogMessage("Witcher moved down using [S]");
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);   
        }
    } 
}