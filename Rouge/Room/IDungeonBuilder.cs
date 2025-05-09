namespace Rouge;

public interface IDungeonBuilder<Template>
{
    void BuildEmptyDungeon(); // Puste podziemia
    void BuildFilledDungeon(); // Wypełnione podziemia
    void AddPaths(); // Dodanie ścieżek
    void AddRooms(); // Dodanie komnat
    void AddCentralRoom(); // Dodanie centralnego pomieszczenia
    void AddItems(); // Dodanie przedmiotów
    void AddWeapons(); // Dodanie broni
    void AddModifiedWeapons(); // Dodanie broni z modyfikatorami
    void AddPotions(); // Dodanie eliksirów
    void AddEnemies(); // Dodanie przeciwników
    Template GetResult(); // Zwraca gotowy labirynt
}


