using System.Text.Json;
using System.Text.Json.Serialization;
using Rouge.Items;

namespace Rouge;

public class ItemListConverter : JsonConverter<List<IItem>>
{
    public override List<IItem> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var items = JsonSerializer.Deserialize<List<JsonElement>>(ref reader, options);
        
        if (items == null)
        {
            Console.WriteLine("BŁĄD: Nie udało się deserializować listy przedmiotów.");
            return new List<IItem>(); // 📌 Unikamy `null`
        }
        
        var result = new List<IItem>();

        foreach (var itemElement in items)
        {
            string itemJson = itemElement.GetRawText();
            result.Add(ItemFactory.DeserializeItem(itemJson)); // ItemFactory
        }
        return result;
    }

    public override void Write(Utf8JsonWriter writer, List<IItem> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}