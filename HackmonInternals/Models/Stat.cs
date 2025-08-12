using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace HackmonInternals.Models;

[UsedImplicitly]
public class Stat
{
   public float GrowthPerLevel { get; set; }
   public int BaseValue { get; set; }

   public int GetValue(int level)
   {
      var baseAdditiveBonus = Modifiers.Aggregate(0, (acc, x) => acc + x.BaseAdditiveBonus);
      var multiplicativeBonus = Modifiers.Aggregate<Modifier, double>(1, (acc, x) => acc + x.Multiplier);
      var stat = (BaseValue + baseAdditiveBonus + (GrowthPerLevel * level)) * multiplicativeBonus;
      return (int)Math.Round(stat);
   }
   
   [JsonIgnore]
   public List<Modifier> Modifiers{ get; set; } = [];
}