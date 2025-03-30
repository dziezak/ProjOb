namespace Rouge.ActionHandler.Handlers;

public class DropItemHandler:ActionHandlerBase
{
    public DropItemHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'o')
        {
            GameDisplay.Instance?.AddLogMessage("Probuje zdjac przedmiot");
            player.lastCharacter = 'o';
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }

    }
}