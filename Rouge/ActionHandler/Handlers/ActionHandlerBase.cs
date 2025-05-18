namespace Rouge.ActionHandler;

public class ActionHandlerBase : IActionHandler
{
    protected IActionHandler _nextHandler;

    public void SetNext(IActionHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    public virtual void Handle(char input, Room room, Player player)
    {
        if (_nextHandler != null)
        {
            _nextHandler.Handle(input, room, player);
        }
        else
        {
            GameDisplay.Instance?.AddLogMessage($"Unhandled input: {input}");
        }
    }
    public static void UpdateUI(Room room, Player player)
    {
        player.ItemsToGetFromRoom = room.GetItemsAt(player.X, player.Y);
        player.WarningMessage = "";
       /* 
        player.ShowStats(room, player);
        if (player.LogMessage.Length > 0)
            GameDisplay.Instance?.AddLogMessage(player.LogMessage);
        GameDisplay.Instance?.DisplayLog(16, room.Width);
        */
        player.LogMessage = "";
        
    }
}