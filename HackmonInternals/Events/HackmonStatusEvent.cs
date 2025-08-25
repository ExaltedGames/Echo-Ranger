using HackmonInternals.StatusEffects;

namespace HackmonInternals.Events;

public class HackmonStatusEvent(HackmonInstance u, Status s, int n) : HackmonBattleEvent
{
	public HackmonInstance Unit { get; set; } = u;
	public Status Status { get; set; } = s;
	public int Stacks { get; set; } = n;
}