namespace HackmonInternals.StatusEffects;

public class AttackUp : Status
{
	private const int STACK_LIMIT = 8;

	// TODO Nullability needs to be properly determined, across all types with _statMod
	private readonly Modifier _statMod = null!;

	[Status("AttackUp")]
	public static AttackUp Init(HackmonInstance unit, int nTurns) => new(unit, nTurns);

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

		_statMod = new()
		{
			Multiplier = Stacks * 0.05,
			BaseAdditiveBonus = unit.Level
		};

		unit.StatModifiers[StatType.Attack].Add(_statMod);
	}

	public override void Add(int stacks)
	{
		Stacks = Math.Min(STACK_LIMIT, Stacks + stacks);
		_statMod.Multiplier = Stacks * 0.05;
	}

	public override void Remove(int stacks)
	{
		Stacks = Math.Max(0, Stacks - stacks);
		if (Stacks == 0)
			Unit.StatModifiers[StatType.Attack].Remove(_statMod);
	}
}