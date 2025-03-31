namespace Rouge.ActionHandler.Handlers;

public class PickUpItemNumberHandler:ActionHandlerBase
{

    public PickUpItemNumberHandler()
    {
    }
    public override void Handle(char itemToPickUp, Room room, Player player) 
    {
        GameDisplay.Instance?.DisplayMovementInformation("", room);
        if (player.lastCharacter != 'p')
        {
            base.Handle(itemToPickUp, room, player);
            //UpdateUI(room, player);
            return; // wykonalem swoja prace i konicze
        }
        if (char.IsDigit(itemToPickUp))
        {
            player.lastCharacter = itemToPickUp;
            player.PickUpItem(int.Parse(itemToPickUp.ToString()));
            UpdateUI(room, player);
        }
        else
        {
            player.WarningMessage += "Invalid input. Please enter a digit.\n";
            UpdateUI(room, player);
            base.Handle(itemToPickUp, room, player);
        }
    }
}