using HackmonInternals.StatusEffects;

namespace HackmonInternals.Events;

public class HackmonStatusEvent : HackmonBattleEvent
{
    public HackmonBattleEvent Unit { get; set; }
    public Status Status { get; set; }
    public int Stacks { get; set; }

    public HackmonStatusEvent(HackmonBattleEvent u, Status s, int n)
    {
        Unit = u;
        Status = s;
        Stacks = n;
    }
}