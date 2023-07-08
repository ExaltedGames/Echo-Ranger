using HackmonInternals.Battle;
using HackmonInternals.Models;

namespace HackmonInternals.Events.Battle;

public class MonLeftBattle : BattleEvent
{
    public HackmonInstance Hackmon;
    
    public MonLeftBattle(BattleManager manager, HackmonInstance hackmon) : base(manager)
    {
        Hackmon = hackmon;
    }
}