using JetBrains.Annotations;

namespace HackmonInternals.Models;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Modifier
{
	public int BaseAdditiveBonus { get; set; } = 0;
	/// <summary>
	/// Negative mods need to be negative numbers and vice versa. 0 = no change.
	/// </summary>
	public double Multiplier { get; set; } = 0;
}