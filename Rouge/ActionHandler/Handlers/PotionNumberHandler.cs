namespace Rouge.ActionHandler.Handlers;

public class PotionNumberHandler : ActionHandlerBase
{
    public PotionNumberHandler()
    {
    }

    public override void Handle(char input, Room room, Player player)
    {
        if (char.IsDigit(input) && player.lastCharacter == 'e')
        {
            player.lastCharacter = input;
            player.DrinkPotionFromInventory(input, room);
            UpdateUI(room, player);
        }
        else
        {
            base.Handle(input, room, player);
        }
    } 
}