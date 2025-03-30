namespace Rouge.ActionHandler.Handlers;

public class PickUpItemNumberHandler:ActionHandlerBase
{
    private readonly PickUpItemHandler _previousHandler;

    public PickUpItemNumberHandler(PickUpItemHandler previousHandler)
    {
        _previousHandler = previousHandler;
    }
    public override void Handle(char itemToPickUp, Room room, Player player) 
    {
        if (_previousHandler.IsActivated() == false)
        {
            base.Handle(itemToPickUp, room, player);
            //player.WarningMessage = "Didnt work.\n"; 
            UpdateUI(room, player);
            return; // wykonalem swoja prace i konicze
        }
        if (char.IsDigit(itemToPickUp))
        {
            player.PickUpItem(int.Parse(itemToPickUp.ToString()));
            _previousHandler.Deactivate();
            UpdateUI(room, player);
            //_previousHandler.Handle('?', room, player);
        }
        else
        {
            player.WarningMessage += "Invalid input. Please enter a digit.\n";
            UpdateUI(room, player);
            base.Handle(itemToPickUp, room, player);
        }
    }
}