using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace HackmonInternals.Models;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class HackmonMove
{
	public required HackmonType MoveType { get; set; }
	public required string Name { get; set; }
	public required int Id { get; set; }
	public required string Description { get; set; }
	public required int Damage { get; set; }
	public required AttackType AttackType { get; set; }
	public required int StaminaCost { get; set; }
	public List<StatusDescriptor> UserStatuses { get; set; } = [];
	public List<StatusDescriptor> TargetStatuses { get; set; } = [];

	[JsonIgnore]
	public List<Type> UserStatusTypes { get; set; } = [];

	[JsonIgnore]
	public List<Type> TargetStatusTypes { get; set; } = [];
}