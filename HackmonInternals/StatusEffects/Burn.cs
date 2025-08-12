using HackmonInternals.Attributes;
using HackmonInternals.Models;
using JetBrains.Annotations;
using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Events;

namespace HackmonInternals.StatusEffects;

[UsedImplicitly]
public class Burn : Status
{
    public const int STACK_LIMIT = 8;
    
    [Status("Burn")]
    public static Burn Init(HackmonInstance unit, int nTurns)
    {
        return new Burn(unit, nTurns);
    }
    
    public Burn(HackmonInstance unit, int stacks) : base(unit, stacks)
    {
        unit.StatusEffects.Add(this);
        BattleManager.OnTurnEnd += DoTick;
    }

    public override void Add(int stacks)
    {
        Stacks = Math.Min(STACK_LIMIT, Stacks + stacks);
    }

    public override void Remove(int stacks)
    {
        Stacks = Math.Max(0, Stacks - stacks);
        if (Stacks == 0)
        {
            Unit.StatusEffects.Remove(this);
        }
    }

    private void DoTick(EndTurnEvent e)
    {
        Unit.Health -= (int)Math.Round(Unit.MaxHp*0.05);
        Remove(1); 
    }
}