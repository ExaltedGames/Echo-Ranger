namespace HackmonInternals.Models;

public class StatusDescriptor
{
    public string Name { get; set; }
    public int Duration { get; set; }

    public StatusDescriptor(string name, int duration)
    {
        Name = name;
        Duration = duration;
    }
}