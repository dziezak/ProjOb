namespace Rouge.ActionHandler.Handlers;

public class PotionHandHandler : ActionHandlerBase
{
    public PotionHandHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'r' || input == 'l')
        {
            player.PotionFunction(input);
        }
        else
        {
            base.Handle(input, room, player);
        }
    }
}