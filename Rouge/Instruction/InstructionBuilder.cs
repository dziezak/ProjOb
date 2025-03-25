using System.Text;

namespace Rouge.Instruction;

public class InstructionBuilder : IInstructionBuilder
{
    private StringBuilder _instructions;

    public InstructionBuilder()
    {
        _instructions = new StringBuilder();
    }

    public void AddMovementInstructions()
    {
        _instructions.AppendLine("[W] - Move Up");
        _instructions.AppendLine("[A] - Move Left");
        _instructions.AppendLine("[S] - Move Down");
        _instructions.AppendLine("[D] - Move Right");
    }

    public void AddItemPickupInstructions()
    {
        _instructions.AppendLine("[P] - Pick Up Item (then pick number from 0-9)");
        _instructions.AppendLine("[R] - Equip Item in Right Hand (then pick number from 0-9)");
        _instructions.AppendLine("[L] - Equip Item in Left Hand (then pick number from 0-9)");
        _instructions.AppendLine("[O] - Drop Item (choose hand: 'r' or 'l')");
        _instructions.AppendLine("[M] - Drop All Items from inventory");
    }

    public void AddCombatInstructions()
    {
    }

    public void AddWeaponInstructions()
    {
        //future for fight
    }

    public void AddPotionInstructions()
    {
        _instructions.AppendLine("[E] - Use Potion in your hand (then choose hand: 'r' or 'l')");
    }

    public string GetInstructions()
    {
        return _instructions.ToString();
    }
}
