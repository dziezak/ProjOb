using System.Text;

namespace Rouge;

public class LegendBuilder : IDungeonBuilder<string>
{
    private StringBuilder _legend; 

    public LegendBuilder()
    {
        _legend = new StringBuilder();
    }

    public void Reset()
    {
        _legend.Clear();
    }

    public void BuildEmptyDungeon()
    {
        _legend.AppendLine("╔══════════════════════════════╗");
        _legend.AppendLine("║       WHITCHERS MOVES:       ║");
        _legend.AppendLine("╚══════════════════════════════╝");
        _legend.AppendLine("Available keys:A");
        _legend.AppendLine("[W] - Move Up");
        _legend.AppendLine("[A] - Move Left");
        _legend.AppendLine("[S] - Move Right");
        _legend.AppendLine("[D] - Move Right");
    }

    public void BuildFilledDungeon()
    {
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
        _legend.AppendLine("[P] - Pick Up Item (then pick number from 0-9) ");
    }

    public void AddWeapons()
    {
        _legend.AppendLine("[R] - Equip Item in Right Hand (then pick number from 0-9)");
        _legend.AppendLine("[L] - Equip Item in Left Hand (then pick number from 0-9)");
        _legend.AppendLine("[O] - Drop Item (choose hand: 'r' or 'l') or from invetory (choose 0-9)");
        _legend.AppendLine("[M] - Drop All Items from inventory");
    }

    public void AddModifiedWeapons()
    {
        _legend.AppendLine("[R] - Equip Item in Right Hand (then pick number from 0-9)");
        _legend.AppendLine("[L] - Equip Item in Left Hand (then pick number from 0-9)");
        _legend.AppendLine("[O] - Drop Item (choose hand: 'r' or 'l')");
        _legend.AppendLine("[M] - Drop All Items from inventory");
    }

    public void AddPotions()
    {
        _legend.AppendLine("[E] - Use Potion (then choose hand: 'r' or 'l' or number 0-9 from inventory)");
    }

    public void AddEnemies()
    {
    }

    public string GetResult()
    {
        return _legend.ToString();
    }
}