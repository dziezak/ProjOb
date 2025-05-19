using System;
using System.Collections.Generic;
using System.Text.Json;
using Rouge.Items;

public static class ItemFactory
{
    private static readonly Dictionary<string, Func<IItem, IItem>> ItemDecorators = new()
    {
        { "Lucky_", item => new LuckyItemDecorator(item) },
        { "Powerful_", item => new PowerfulItemDecorator(item) },
        { "Pityful_", item => new PitifulItemDecorator(item) },
        { "Heavy_", item => new HeavyItemDecorator(item) },
        { "Useless_", item => new UselessItemDecorator(item) }
    };
    
    public static IItem DeserializeItem(string json)
    {
        try
        {
            var jsonObject = JsonDocument.Parse(json).RootElement;

            // 📌 Pobieramy nazwę przedmiotu, aby ustalić jego typ
            if (jsonObject.TryGetProperty("name", out var nameElement))
            {
                string itemName = nameElement.GetString() ?? "";

                // 📌 Sprawdzamy, czy przedmiot pasuje do dekoratora
                foreach (var decorator in ItemDecorators)
                {
                    if (itemName.StartsWith(decorator.Key))
                    {
                        var baseItem = JsonSerializer.Deserialize<IItem>(json);
                        return baseItem != null ? decorator.Value(baseItem) : throw new JsonException("Nie można odczytać przedmiotu.");
                    }
                }

                // 📌 Jeśli nie pasuje do dekoratora, używamy domyślnego deserializatora
                return JsonSerializer.Deserialize<IItem>(json) ?? throw new JsonException("Nie można odczytać przedmiotu.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"BŁĄD deserializacji przedmiotu: {ex.Message}");
        }

        return null!; // 📌 Zwracamy `null`, jeśli deserializacja się nie powiedzie
    }

}