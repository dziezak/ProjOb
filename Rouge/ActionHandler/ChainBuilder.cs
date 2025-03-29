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
        AddHandler(new DropItemHandler());
        AddHandler(new DropItemNumberHandler());

        AddHandler(new GetItemToLeftHandHandler());
        AddHandler(new GetItemToLeftHandNumberHandler());
        
        AddHandler(new GetItemToRightHandHandler());
        AddHandler(new GetItemToRightHandNumberHandler());
    }

    public void AddPotions()
    {
        AddHandler(new PotionHandler());
        AddHandler(new PotionHandHandler());
    }

    public void AddEnemies()
    {
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
