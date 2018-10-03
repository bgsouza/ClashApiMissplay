using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clash {
  public class Items {
    [JsonProperty (PropertyName = "participants")]
    public List<WarParticipants> Participants { get; set; }
    [JsonProperty (PropertyName = "name")]
    public string Name { get; set; }
  }

  public class ItemsList{
    [JsonProperty (PropertyName = "items")]
    public List<Items> Items { get; set; }
  }
}