using Rouge.ActionHandler;
using Rouge.ActionHandler.Handlers;

namespace Rouge;

public class ChainBuilder : IDungeonBuilder<IActionHandler>
{
    private IActionHandler _firstHandler; // Pierwszy handler w łańcuchu
    private IActionHandler _currentHandler; // Ostatni dodany handler
    private Player _player;
    private Room _room;

    public ChainBuilder(Player player, Room room)
    {
        _player = player;
        _room = room;
    }

    public void BuildEmptyDungeon()
    {
    }

    public void BuildFilledDungeon()
    {
        AddHandler(new MoveUpHandler());
        AddHandler(new MoveDownHandler());
        AddHandler(new MoveLeftHandler());
        AddHandler(new MoveRightHandler());
        AddHandler(new EndGameHandler());
    }

    public void AddPaths()
    {
    }

    public void AddRooms()
    {
    }

    public void AddCentralRoom()
    {
    }

    public void AddItems()
    {
        AddHandler(new PickUpItemHandler());
        AddHandler(new PickUpItemNumberHandler());
    }

    public void AddWeapons()
    {
    }

    public void AddModifiedWeapons()
    {

        AddHandler(new GetItemToLeftHandHandler());
        AddHandler(new GetItemToLeftHandNumberHandler());
        AddHandler(new GetItemToRightHandHandler());
        AddHandler(new GetItemToRightHandNumberHandler());
        AddHandler(new DropItemHandler());
        AddHandler(new DropItemHandHandler());
        AddHandler(new DropItemNumberHandler());
        AddHandler(new DropAllItemsFromInventoryHandler());
    }

    public void AddPotions()
    {
        AddHandler(new PotionHandler());
        AddHandler(new PotionInfoHandler());
        AddHandler(new PotionHandHandler());
        AddHandler(new PotionNumberHandler());
    }

    public void AddEnemies()
    {
        AddHandler(new EnemySelectHandler());
        AddHandler(new ChooseEnemyHandler());
        AddHandler(new EnemySelectNumberHandler());
    }

    public IActionHandler GetResult()
    {
        AddHandler(new GuardHandler());
        return _firstHandler;
    }

    public void AddHandler(IActionHandler handler)
    {
        if (_firstHandler == null)
        {
            _firstHandler = handler; // Ustawiamy pierwszy handler
            _currentHandler = handler;
        }
        else
        {
            _currentHandler.SetNext(handler); // Łączymy handler w łańcuch
            _currentHandler = handler; // Aktualizujemy aktualny
        }
    }
}
