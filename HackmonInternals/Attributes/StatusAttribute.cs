namespace HackmonInternals.Attributes;

[System.AttributeUsage(System.AttributeTargets.Method)]
public class StatusAttribute : System.Attribute
{
    public string Name;

    public StatusAttribute(string name)
    {
        Name = name;
    }
}