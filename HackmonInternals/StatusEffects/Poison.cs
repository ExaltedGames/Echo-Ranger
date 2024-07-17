﻿using HackmonInternals.Attributes;
using HackmonInternals.Models;
using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Events;

namespace HackmonInternals.StatusEffects;

public class Poison : Status
{
    private static readonly int STACK_LIMIT = 8;
    
    [Status("Poison")]
    public static Poison Init(HackmonInstance unit, int numTurns)
    {
        return new Poison(unit, numTurns);
    }
    
    public Poison(HackmonInstance unit, int numTurns) : base(unit, numTurns)
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