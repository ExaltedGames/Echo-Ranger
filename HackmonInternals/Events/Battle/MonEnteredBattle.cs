using HackmonInternals.Battle;
using HackmonInternals.Models;

namespace HackmonInternals.Events.Battle;

public class MonEnteredBattle : BattleEvent
{
    public HackmonInstance Hackmon;
    
    public MonEnteredBattle(BattleManager manager, HackmonInstance hackmon) : base(manager)
    {
        Hackmon = hackmon;
    }
}