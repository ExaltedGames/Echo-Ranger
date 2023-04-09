using System.Text.Json.Serialization;

namespace HackmonInternals;

public class Stat
{
   [JsonIgnore]
   public int Value { get; set; }
   
   public float GrowthPerLevel { get; set; }
   public int BaseValue { get; set; }
   
   [JsonIgnore]
   public List<Modifier> Modifiers { get; set; } = new();
}