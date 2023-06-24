using HackmonInternals.Battle;

namespace HackmonInternals.Events.Battle;

public class TurnEvent : BattleEvent
{
    public TurnEvent(BattleManager manager) : base(manager)
    {
    }
}