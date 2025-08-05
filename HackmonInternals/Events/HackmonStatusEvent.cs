using HackmonInternals.Models;
using HackmonInternals.StatusEffects;

namespace HackmonInternals.Events;

public class HackmonStatusEvent : HackmonBattleEvent
{
    public HackmonInstance Unit { get; set; }
    public Status Status { get; set; }
    public int Stacks { get; set; }

    public HackmonStatusEvent(HackmonInstance u, Status s, int n)
    {
        Unit = u;
        Status = s;
        Stacks = n;
    }
}