﻿namespace HackmonInternals.StatusEffects;

public class SpAttackUp : Status
{
    public override string Name { get; set; } = "SpAttackUp";

    public SpAttackUp(int numTurns) : base(numTurns)
    {
    }
}