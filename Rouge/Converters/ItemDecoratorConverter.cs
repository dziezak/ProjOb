using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Rouge.Items;

public class ItemDecoratorConverter : JsonConverter<IItem>
{
    public override IItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
        var decoratedItemJson = jsonObject.GetProperty("DecoratedItem").GetRawText();
        
        IItem baseItem = JsonSerializer.Deserialize<IItem>(decoratedItemJson, options); // 📌 Deserializujemy bazowy przedmiot

        if (jsonObject.TryGetProperty("GetName", out _))
        {
            string itemName = baseItem.GetName();

            return 
                itemName.StartsWith("Lucky_") ? new LuckyItemDecorator(baseItem) :
                itemName.StartsWith("Powerful_") ? new PowerfulItemDecorator(baseItem) :
                itemName.StartsWith("Pityful_") ? new PitifulItemDecorator(baseItem) :
                itemName.StartsWith("Heavy_") ? new HeavyItemDecorator(baseItem) :
                itemName.StartsWith("Useless_") ? new UselessItemDecorator(baseItem) :
                baseItem; // Jeśli nie pasuje do dekoratora, zwracamy bazowy przedmiot
        }

        return baseItem;
    }

    public override void Write(Utf8JsonWriter writer, IItem value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}