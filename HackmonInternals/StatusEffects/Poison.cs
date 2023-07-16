namespace HackmonInternals.StatusEffects;

public class Poison : Status
{
    public override string Name { get; set; } = "Poison";

    public Poison(int numTurns) : base(numTurns)
    {
    }
}