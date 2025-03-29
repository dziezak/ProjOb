namespace Rouge;

public class DungeonDirector
{
    public DungeonDirector() {}

    public void BuildBasicDungeon<Template>(IDungeonBuilder<Template> builder)
    {
        builder.BuildEmptyDungeon();
        builder.BuildFilledDungeon();
        builder.AddPaths();
        builder.AddCentralRoom();
        builder.AddRooms();
    }

    public void BuildFilledDungeonWithRooms<Template>(IDungeonBuilder<Template> builder)
    {
        builder.BuildEmptyDungeon();
        builder.BuildFilledDungeon();
        builder.AddPaths();
        builder.AddCentralRoom();
        builder.AddRooms();
        builder.AddItems();
        //builder.AddModifiedWeapons();
        //builder.AddPotions();
        //builder.AddEnemies();
    }
}