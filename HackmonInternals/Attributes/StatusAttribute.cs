using JetBrains.Annotations;

namespace HackmonInternals.Attributes;

[AttributeUsage(AttributeTargets.Method)]
[MeansImplicitUse]
public class StatusAttribute(string name) : Attribute
{
	public readonly string Name = name;
}