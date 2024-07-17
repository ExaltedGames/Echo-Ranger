﻿using System.ComponentModel;
using HackmonInternals.Attributes;
using HackmonInternals.Enums;
using HackmonInternals.Models;

namespace HackmonInternals.StatusEffects;

public class AttackUp : Status
{
    private static readonly int STACK_LIMIT = 8;
    private Modifier StatMod;
    
    [Status("AttackUp")]
    public static AttackUp Init(HackmonInstance unit, int nTurns)
    {
        return new AttackUp(unit, nTurns);
    }

    public AttackUp(HackmonInstance unit, int nTurns) : base(unit, nTurns)
    {
        var oppositeEffect = unit.StatusEffects.Find(effect => effect.Name == "AttackDown");
        if (oppositeEffect != null)
        {
            if (nTurns - oppositeEffect.Stacks <= 0)
            {
                oppositeEffect.Remove(nTurns);
                return;
            }

            Stacks = nTurns - oppositeEffect.Stacks;
        }

        StatMod = new()
        {
            Multiplier = (Stacks * 0.05),
            BaseAdditiveBonus = unit.Level
        };
        unit.StatModifiers[StatType.Attack].Add(StatMod);
    }

    public override void Add(int stacks)
    {
        Stacks = Math.Min(STACK_LIMIT, Stacks + stacks);
        StatMod.Multiplier = (Stacks * 0.05);
    }

    public override void Remove(int stacks)
    {
        Stacks = Math.Max(0, Stacks - stacks);
        if (Stacks == 0)
        {
            Unit.StatModifiers[StatType.Attack].Remove(StatMod);
        }
    }
}