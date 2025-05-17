namespace Rouge;
using System.Text.Json.Serialization;

public class PlayerAction
{
    [JsonPropertyName("playerId")]
    public int PlayerId { get; set; }

    [JsonPropertyName("type")]
    public char Type { get; set; } //'a', 'w', 'p', itd.

    [JsonPropertyName("number")]
    public int? Number { get; set; } // dodatkowa obsluga

    [JsonPropertyName("targetId")]
    public int? TargetId { get; set; } // Jeśli akcja to "Attack" w przyszlosci dla atakowania jakiegos gracza

    public PlayerAction(int playerId, char type, int? number = null, int? targetId = null)
    {
        PlayerId = playerId;
        Type = type;
        Number = Number;
        TargetId = targetId;
    }
}
