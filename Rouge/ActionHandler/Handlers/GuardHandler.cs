namespace Rouge.ActionHandler.Handlers;

public class GuardHandler : ActionHandlerBase
{
   private string _errorMessage;

   public GuardHandler(string errorMessage = "Invalid key pressed.")
   {
      _errorMessage = errorMessage;
   }

   public override void Handle(char input, Room room, Player player)
   {
      GameDisplay.Instance?.AddLogMessage($"{_errorMessage} Key: '{input}'");
   }
}