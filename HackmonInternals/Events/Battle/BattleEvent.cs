using HackmonInternals.Battle;

namespace HackmonInternals.Events.Battle;

public abstract class BattleEvent : HackmonEvent
{
    public BattleManager Manager;

    protected BattleEvent(BattleManager manager)
    {
        Manager = manager;
    }
}