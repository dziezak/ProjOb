namespace Rouge.ActionHandler.Handlers;

public class PotionHandHandler : ActionHandlerBase
{
    public PotionHandHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if ((input == 'l' || input == 'r') && player.lastCharacter == 'e')
        {
            player.lastCharacter = input;
            player.PotionFunction(input);
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}