using HackmonInternals.Attributes;
using HackmonInternals.Models;

namespace HackmonInternals.StatusEffects;

public class Burn : Status
{
    [Status("Burn")]
    public static Burn Init(HackmonInstance unit, int nTurns)
    {
        return new Burn(unit, nTurns);
    }
    
    public Burn(HackmonInstance unit, int numTurns) : base(unit, numTurns)
    {
    }

    public override void Remove(int stacks)
    {
        throw new NotImplementedException();
    }
}