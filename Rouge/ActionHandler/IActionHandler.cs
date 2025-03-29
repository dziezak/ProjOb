namespace Rouge.ActionHandler;

public interface IActionHandler
{
    void SetNext(IActionHandler nextHandler);
    void Handle(char input, Room room, Player player);
}