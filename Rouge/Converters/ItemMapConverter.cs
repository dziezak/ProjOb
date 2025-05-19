using Rouge.Items;

namespace Rouge;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ItemMapConverter : JsonConverter<Dictionary<string, List<IItem>>>
{
    public override Dictionary<string, List<IItem>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, List<IItem>>>(ref reader, options);
        return jsonDict;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, List<IItem>> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
