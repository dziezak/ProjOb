namespace Rouge.ActionHandler.Handlers;

public class PotionInfoHandler: ActionHandlerBase
{
    public PotionInfoHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (input == 'i' && player.lastCharacter == 'e')
        {
            //GameDisplay.Instance?.DisplayMovementInformation("", room);
            player.lastCharacter = input;
            //GameDisplay.Instance?.DisplayStats(room, player, true);
        }
        else
        {
            base.Handle(input, room, player);
        }

    }
}