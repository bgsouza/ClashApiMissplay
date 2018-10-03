using Newtonsoft.Json;

namespace Clash {
  public class Member {
    [JsonProperty (PropertyName = "name")]
    public string Name { get; set; }
  }
}