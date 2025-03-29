namespace Rouge.ActionHandler.Handlers;

public class PickUpItemNumberHandler:ActionHandlerBase
{
    
    public override void Handle(char itemToPickUp, Room room, Player player) 
    {
        if (char.IsDigit(itemToPickUp))
        {
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