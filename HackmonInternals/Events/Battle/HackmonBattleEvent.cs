using HackmonInternals.Battle;
using HackmonInternals.Models;

namespace HackmonInternals.Events.Battle;

public abstract class HackmonBattleEvent : BattleEvent
{
    public HackmonInstance Hackmon;
        
    protected HackmonBattleEvent(BattleManager manager, HackmonInstance hackmon) : base(manager)
    {
        Hackmon = hackmon;
    }
}