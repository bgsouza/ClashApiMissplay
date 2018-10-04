using Newtonsoft.Json;

namespace Clash {
  public class WarParticipants {
    [JsonProperty (PropertyName = "name")]
    public string Name { get; set; }
    [JsonProperty (PropertyName = "battlesPlayed")]
    public int WarBattlesPlayed { get; set; }
    [JsonProperty (PropertyName = "wins")]
    public int WarWins { get; set; }
    [JsonProperty (PropertyName = "collectionDayBattlesPlayed")]
    public int CollectionDayBattlesPlayed { get; set; }
  }
}