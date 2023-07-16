using HackmonInternals.Battle;
using HackmonInternals.Models;
using HackmonInternals.StatusEffects;

namespace HackmonInternals.Events.Battle;

public class GainStatus : BattleEvent
{
    public Status Status;
    public HackmonInstance Hackmon;
    
    public GainStatus(BattleManager manager, HackmonInstance mon, Status status) : base(manager)
    {
        Hackmon = mon;
        Status = status;
    }
}