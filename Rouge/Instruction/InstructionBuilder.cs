using System.Text;

namespace Rouge;

public class InstructionBuilder : IDungeonBuilder<string>
{
    private StringBuilder _legend;

    public InstructionBuilder()
    {
        _legend = new StringBuilder();
    }

    public void Reset()
    {
        _legend.Clear(); 
    }

    public void BuildEmptyDungeon()
    {
        //_legend.AppendLine("The dungeon is empty. Feel free to explore.");
    }

    public void BuildFilledDungeon()
    {
        _legend.AppendLine("The dungeon is filled with walls. Look for paths.");
    }

    public void AddPaths()
    {
        _legend.AppendLine("Paths have been added. Follow them to navigate.");
    }

    public void AddRooms()
    {
        _legend.AppendLine("Rooms have been created in the dungeon.");
    }

    public void AddCentralRoom()
    {
        _legend.AppendLine("A central room has been placed in the dungeon.");
    }

    public void AddItems()
    {
        _legend.AppendLine("Pick up items scattered across the dungeon.");
    }

    public void AddWeapons()
    {
        _legend.AppendLine("Equip weapons in your right hand.");
        _legend.AppendLine("Equip weapons in your left hand.");
    }

    public void AddModifiedWeapons()
    {
        _legend.AppendLine("Modified weapons with efects affecting your stats are available.");
    }

    public void AddPotions()
    {
        _legend.AppendLine("Use potions to gain temporary bonuses.");
    }

    public void AddEnemies()
    {
        _legend.AppendLine("Beware of enemies lurking around.");
    }

    public string GetResult()
    {
        return _legend.ToString();
    }
}

