namespace Rouge.ActionHandler.Handlers;

public class PotionHandler: ActionHandlerBase
{
    public PotionHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
       if(input == 'e')
       {
           player.lastCharacter = input;
           GameDisplay.Instance?.DisplayMovementInformation("Pick number or hand to use potion", room);
           UpdateUI(room, player);
       }
       else
       {
           base.Handle(input, room, player);
       }
    }
}