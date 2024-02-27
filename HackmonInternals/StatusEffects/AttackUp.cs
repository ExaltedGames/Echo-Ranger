using System.ComponentModel;
using HackmonInternals.Attributes;
using HackmonInternals.Models;

namespace HackmonInternals.StatusEffects;

public class AttackUp : Status
{
    [Status("AttackUp")]
    public static AttackUp Init(HackmonInstance unit, int nTurns)
    {
        return new AttackUp(unit, nTurns);
    }

    public AttackUp(HackmonInstance unit, int nTurns) : base(unit, nTurns)
    {
        
    }

    public override void Remove(int stacks)
    {
        throw new NotImplementedException();
    }
}