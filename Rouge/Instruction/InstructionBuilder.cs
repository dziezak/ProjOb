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
        /*
        _legend.AppendLine("------------------------------------------------");
        _legend.AppendLine("⚠️  THE DUNGEON IS EMPTY! TIME TO EXPLORE. ⚠️");
        _legend.AppendLine("------------------------------------------------");
        */
    }

    public void BuildFilledDungeon()
    {
        _legend.AppendLine("------------------------------------------------");
        _legend.AppendLine("🧱  THE DUNGEON IS FILLED WITH WALLS! 🧱");
        _legend.AppendLine("🔎 LOOK FOR PATHS TO FIND YOUR WAY OUT.");
        _legend.AppendLine("------------------------------------------------");
    }

    public void AddPaths()
    {
        _legend.AppendLine("➡️  PATHS HAVE BEEN ADDED TO THE DUNGEON.");
        _legend.AppendLine("🔀 FOLLOW THEM TO NAVIGATE SAFELY.");
        _legend.AppendLine();
    }

    public void AddRooms()
    {
        _legend.AppendLine("🏰  ROOMS HAVE BEEN ADDED.");
        _legend.AppendLine("💎 EXPLORE THEM FOR TREASURES AND SURPRISES.");
        _legend.AppendLine();
    }

    public void AddCentralRoom()
    {
        _legend.AppendLine("✨  A CENTRAL ROOM HAS BEEN PLACED.");
        _legend.AppendLine("💡 CHECK IT OUT FOR SECRET ITEMS OR CLUES.");
        _legend.AppendLine();
    }

    public void AddItems()
    {
        _legend.AppendLine("🎒 PICK UP ITEMS SCATTERED ACROSS THE DUNGEON.");
        _legend.AppendLine("📦 KEEP YOUR INVENTORY ORGANIZED TO SURVIVE.");
        _legend.AppendLine();
    }

    public void AddWeapons()
    {
        _legend.AppendLine("⚔️ EQUIP WEAPONS IN YOUR RIGHT HAND.");
        _legend.AppendLine("🛡️ EQUIP WEAPONS IN YOUR LEFT HAND.");
        _legend.AppendLine("⚙️ USE WEAPONS WISELY TO FIGHT ENEMIES.");
        _legend.AppendLine();
    }

    public void AddModifiedWeapons()
    {
        _legend.AppendLine("🔧 MODIFIED WEAPONS ARE SCATTERED AROUND.");
        _legend.AppendLine("🔮 THEY CAN BOOST YOUR STATS SIGNIFICANTLY.");
        _legend.AppendLine();
    }

    public void AddPotions()
    {
        _legend.AppendLine("🧪 USE POTIONS TO GAIN TEMPORARY BONUSES.");
        _legend.AppendLine("⚡ POTIONS CAN HELP YOU TURN THE TIDE IN BATTLES.");
        _legend.AppendLine();
    }

    public void AddEnemies()
    {
        _legend.AppendLine("👾 BEWARE OF ENEMIES LURKING IN THE SHADOWS.");
        _legend.AppendLine("💀 PREPARE YOURSELF BEFORE ENTERING DANGEROUS AREAS.");
        _legend.AppendLine();
    }

    public string GetResult()
    {
        _legend.AppendLine("------------------------------------------------");
        _legend.AppendLine("🔥 GOOD LUCK EXPLORING THE DUNGEON, HERO! 🔥");
        _legend.AppendLine("------------------------------------------------");
        return _legend.ToString();
    }
}


