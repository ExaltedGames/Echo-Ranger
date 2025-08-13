namespace HackmonInternals.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class StatusAttribute(string name) : Attribute
{
    public readonly string Name = name;
}