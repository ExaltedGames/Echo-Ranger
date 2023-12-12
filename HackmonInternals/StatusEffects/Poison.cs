using HackmonInternals.Attributes;
using HackmonInternals.Models;

namespace HackmonInternals.StatusEffects;

public class Poison : Status
{
    [Status("Poison")]
    public static Poison Init(HackmonInstance unit, int numTurns)
    {
        return new Poison(unit, numTurns);
    }
    
    public Poison(HackmonInstance unit, int numTurns) : base(unit, numTurns)
    {
    }

    public override void Remove(int stacks)
    {
        throw new NotImplementedException();
    }
}