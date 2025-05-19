namespace Rouge;
using System.Text.Json;
using System.Text.Json.Serialization;

public class EnemyMapConverter : JsonConverter<Dictionary<string, Enemy>>
{
    public override Dictionary<string, Enemy> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, Enemy>>(ref reader, options);
        return jsonDict;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<string, Enemy> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
