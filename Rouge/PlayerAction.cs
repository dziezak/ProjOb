namespace Rouge;
using System.Text.Json.Serialization;

public class PlayerAction
{
    [JsonPropertyName("playerId")]
    public int PlayerId { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } // "Move", "Attack", "PickUpItem", 

    [JsonPropertyName("number")]
    public int? Number { get; set; } // Jeśli akcja to "Move"

    [JsonPropertyName("targetId")]
    public int? TargetId { get; set; } // Jeśli akcja to "Attack"

    public PlayerAction(int playerId, string type, int? number = null, int? targetId = null)
    {
        PlayerId = playerId;
        Type = type;
        Number = Number;
        TargetId = targetId;
    }
}
