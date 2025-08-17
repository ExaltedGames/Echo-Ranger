using TurnBasedBattleSystem;

namespace HackmonInternals.StatusEffects;

public abstract class Status(HackmonInstance unit, int stacks) : IStatus
{
	public string Name { get; set; } = null!;
	public HackmonInstance Unit { get; set; } = unit;
	public int Stacks = stacks;
	public abstract void Remove(int stacks);
	public abstract void Add(int stacks);
}