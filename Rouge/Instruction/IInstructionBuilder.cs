namespace Rouge.Instruction;

public interface IInstructionBuilder
{
    void AddMovementInstructions();     // Dodaje instrukcje poruszania się
    void AddItemPickupInstructions();  // Dodaje instrukcje podnoszenia przedmiotów
    void AddCombatInstructions();      // Dodaje instrukcje walki (lub przeciwników) // przyszlosciowe
    void AddWeaponInstructions();      // Dodaje instrukcje używania broni
    void AddPotionInstructions();      // Dodaje instrukcje używania mikstur
    string GetInstructions();          // Zwraca pełną instrukcję jako string
}
