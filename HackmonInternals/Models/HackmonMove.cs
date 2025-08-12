using System.Text.Json.Serialization;
using HackmonInternals.Enums;

namespace HackmonInternals.Models;

public class HackmonMove
{
   public HackmonType MoveType { get; set; } 
   public string Name { get; set; }
   public int ID { get; set; }
   public string Description { get; set; }
   public int Damage { get; set; }
   public AttackType AttackType { get; set; }
   public int StaminaCost { get; set; }
   public List<StatusDescriptor> UserStatuses { get; set; } = new();
   public List<StatusDescriptor> TargetStatuses { get; set; } = new();

   [JsonIgnore]
   public List<Type> UserStatusTypes { get; set; } = new();

   [JsonIgnore]
   public List<Type> TargetStatusTypes { get; set; } = new();
}