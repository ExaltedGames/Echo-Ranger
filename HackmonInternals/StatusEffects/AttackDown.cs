using HackmonInternals.Enums;
using HackmonInternals.Models;
using HackmonInternals.Attributes;
using JetBrains.Annotations;

namespace HackmonInternals.StatusEffects;

[UsedImplicitly]
public class AttackDown : Status
{
    private const int STACK_LIMIT = 8;
    private const double MULTIPLIER_PER_STACK = -0.5;
    private Modifier _statMod;

    [Status("AttackDown")]
    public static AttackDown Init(HackmonInstance unit, int nTurns)
    {
        return new AttackDown(unit, nTurns);
    }

    public AttackDown(HackmonInstance unit, int nTurns) : base(unit, nTurns)
    {
        var oppositeEffect = unit.StatusEffects.Find(effect => effect.Name == "AttackUp");
        if (oppositeEffect != null)
        {
            if (nTurns - oppositeEffect.Stacks <= 0)
            {
                oppositeEffect.Remove(nTurns);
                return;
            }

            Stacks = nTurns - oppositeEffect.Stacks;
        }
        _statMod = new()
        {
            Multiplier = (Stacks * MULTIPLIER_PER_STACK),
            BaseAdditiveBonus = unit.Level
        };
        unit.StatModifiers[StatType.Attack].Add(_statMod);
    }

    public override void Add(int stacks)
    {
        Stacks = Math.Min(STACK_LIMIT, Stacks + stacks);
        _statMod.Multiplier = (Stacks * MULTIPLIER_PER_STACK);
    }

    public override void Remove(int stacks)
    {
        Stacks = Math.Max(0, Stacks - stacks);
        if (Stacks == 0)
        {
            Unit.StatModifiers[StatType.Attack].Remove(_statMod);
        }
    }
}