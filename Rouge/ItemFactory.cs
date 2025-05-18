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
        IItem baseItem = JsonSerializer.Deserialize<IItem>(json);

        // 📌 Sprawdzamy, czy przedmiot jest dekorowany
        foreach (var decorator in ItemDecorators)
        {
            if (baseItem.GetName().StartsWith(decorator.Key))
            {
                return decorator.Value(baseItem); // 📌 Tworzymy odpowiedni dekorator
            }
        }

        return baseItem; // 📌 Jeśli nie pasuje do żadnego dekoratora, zwracamy podstawowy przedmiot
    }
}