using HackmonInternals.Attributes;
using HackmonInternals.Models;

namespace HackmonInternals.StatusEffects;

public class SpAttackUp : Status
{
    [Status("SpAttackUp")]
    public static SpAttackUp Init(HackmonInstance unit, int numTurns)
    {
        return new SpAttackUp(unit, numTurns);
    }
    
    public SpAttackUp(HackmonInstance unit, int numTurns) : base(unit, numTurns)
    {
    }

    public override void Remove(int stacks)
    {
        
    }
}