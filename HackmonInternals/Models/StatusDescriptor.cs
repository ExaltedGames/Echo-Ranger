using JetBrains.Annotations;

namespace HackmonInternals.Models;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class StatusDescriptor(string name, int duration)
{
	public string Name { get; set; } = name;
	public int Duration { get; set; } = duration;
}