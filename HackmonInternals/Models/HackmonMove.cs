using System.Text.Json.Serialization;
using HackmonInternals.Enums;
using HackmonInternals.StatusEffects;

namespace HackmonInternals.Models;

public class HackmonMove
{
   public HackmonType MoveType { get; set; } 
   public string Name { get; set; }
   public int ID { get; set; }
   public int Damage { get; set; }
   public AttackType AttackType { get; set; }
   public int StaminaCost { get; set; }
   public List<string> UserStatuses { get; set; } = new();
   public List<string> TargetStatuses { get; set; } = new();

   [JsonIgnore]
   public List<Status> UserStatusList { get; set; } = new();

   [JsonIgnore]
   public List<Status> TargetStatusList { get; set; } = new();
}