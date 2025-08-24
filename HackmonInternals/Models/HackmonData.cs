using JetBrains.Annotations;

namespace HackmonInternals.Models;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class HackmonData
{
	public required int Id { get; set; }
	public required string Name { get; set; }
	public required HackmonType PrimaryType { get; set; }
	public HackmonType? SecondaryType { get; set; }
	public HackmonFamily Family { get; set; }
	public required Stat MaxHp { get; set; }
	public required Stat Attack { get; set; }
	public required Stat SpAttack { get; set; }
	public required Stat Defense { get; set; }
	public required Stat SpDefense { get; set; }
	public required Stat MaxStamina { get; set; }
	public List<int[]> LearnableMoves { get; set; } = [];
}