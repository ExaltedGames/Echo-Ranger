using HackmonInternals.Attributes;
using HackmonInternals.Enums;
using HackmonInternals.Models;
using JetBrains.Annotations;

namespace HackmonInternals.StatusEffects;

[UsedImplicitly]
public class DefenseUp : Status
{
    private const int STACK_LIMIT = 8;
    private Modifier _statMod;

    [Status("DefenseUp")]
    public static DefenseUp Init(HackmonInstance unit, int nTurns)
    {
        return new DefenseUp(unit, nTurns);
    }

    public DefenseUp(HackmonInstance unit, int nTurns) : base(unit, nTurns)
    {
        var oppositeEffect = unit.StatusEffects.Find(effect => effect.Name == "DefenseDown");
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
            Multiplier = (Stacks * 0.05),
            BaseAdditiveBonus = unit.Level
        };
        unit.StatModifiers[StatType.Defense].Add(_statMod);
    }

    public override void Add(int stacks)
    {
        Stacks = Math.Min(STACK_LIMIT, Stacks + stacks);
        _statMod.Multiplier = (Stacks * 0.05);
    }

    public override void Remove(int stacks)
    {
        Stacks = Math.Max(0, Stacks - stacks);
        if (Stacks == 0)
        {
            Unit.StatModifiers[StatType.Defense].Remove(_statMod);
        }
    }
}