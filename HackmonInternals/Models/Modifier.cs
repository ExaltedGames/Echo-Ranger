using JetBrains.Annotations;

namespace HackmonInternals.Models;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class Modifier
{
	public int BaseAdditiveBonus { get; set; } = 0;
	public double Multiplier { get; set; } = 1;
}