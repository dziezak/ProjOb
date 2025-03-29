namespace Rouge.ActionHandler.Handlers;

public class PickUpItemNumberHandler:ActionHandlerBase
{
    public override void Handle(char itemToPickUp, Room room, Player player) 
    {
        if (char.IsDigit(itemToPickUp))
        {
            player.PickUpItem(int.Parse(itemToPickUp.ToString()));
        }
        else
        {
            player.WarningMessage += "Invalid input. Please enter a digit.\n";
            base.Handle(itemToPickUp, room, player);
        }

        UpdateUI(room, player);
    }
}