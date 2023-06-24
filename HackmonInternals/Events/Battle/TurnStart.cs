using HackmonInternals.Battle;

namespace HackmonInternals.Events.Battle;

public class TurnStart : TurnEvent
{
    public TurnStart(BattleManager manager) : base(manager)
    {
    }
}