namespace Rouge;

public class DungeonDirector
{
    private IDungeonBuilder _builder;

    public DungeonDirector(IDungeonBuilder builder)
    {
        _builder = builder;
    }

    public void BuildBasicDungeon()
    {
        _builder.BuildEmptyDungeon();
        _builder.BuildFilledDungeon();
        _builder.AddPaths();
        _builder.AddCentralRoom();
        _builder.AddRooms();
    }

    public void BuildFilledDungeonWithRooms()
    {
        _builder.BuildEmptyDungeon();
        _builder.BuildFilledDungeon();
        _builder.AddPaths();
        _builder.AddCentralRoom();
        _builder.AddRooms();
        _builder.AddItems();
        _builder.AddModifiedWeapons();
        _builder.AddPotions();
        //_builder.AddEnemies();
    }
}