namespace HackmonInternals.StatusEffects;

public class Burn : Status
{
    public override string Name { get; set; } = "Burning";

    public Burn(int numTurns) : base(numTurns)
    {
    }
}