using System.Text.Json.Serialization;
using HackmonInternals.StatusEffects;

namespace HackmonInternals;

public class HackmonMove
{
   public HackmonType MoveType { get; set; } 
   public string Name { get; set; }
   public int Damage { get; set; }
   public int StaminaCost { get; set; }
   public List<string> UserStatuses { get; set; }
   public List<string> TargetStatuses { get; set; }
   
   [JsonIgnore]
   public List<Status> UserStatusList { get; set; }
   
   [JsonIgnore]
   public List<Status> TargetStatusList { get; set; }
}