using HackmonInternals.Battle;

namespace HackmonInternals.Events.Battle;

public class TurnEnd : TurnEvent
{
    public TurnEnd(BattleManager manager) : base(manager)
    {
    }
}