using Newtonsoft.Json;

namespace Clash {
  public class WarParticipants {
    [JsonProperty (PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty (PropertyName = "battlesPlayed")]
    public string WarBattlesPlayed { get; set; }
    [JsonProperty (PropertyName = "wins")]
    public string WarWins { get; set; }
    [JsonProperty (PropertyName = "collectionDayBattlesPlayed")]
    public string CollectionDayBattlesPlayed { get; set; }
  }
}