using HackmonInternals.Attributes;
using HackmonInternals.Models;
using JetBrains.Annotations;
using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Events;

namespace HackmonInternals.StatusEffects;

[UsedImplicitly]
public class Poison : Status
{
    private const int STACK_LIMIT = 8;
    
    [Status("Poison")]
    public static Poison Init(HackmonInstance unit, int stacks)
    {
        return new Poison(unit, stacks);
    }
    
    public Poison(HackmonInstance unit, int stacks) : base(unit, stacks)
    {
        BattleManager.OnTurnEnd += DoTick;
        unit.StatusEffects.Add(this); 
    }

    public override void Add(int stacks)
    {
        Stacks = Math.Min(Stacks + stacks, STACK_LIMIT);
    }

    public override void Remove(int stacks)
    {
        Stacks = Math.Max(0, Stacks - stacks);
        if (Stacks == 0)
        {
            // cleanup time
            BattleManager.OnTurnEnd -= DoTick;
            Unit.StatusEffects.Remove(this);
        }
    }

    private void DoTick(EndTurnEvent e)
    {
        Unit.Health -= (int)Math.Round(Unit.MaxHp * 0.05); 
        Remove(1);
    }
}