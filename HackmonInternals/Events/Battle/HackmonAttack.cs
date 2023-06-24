using HackmonInternals.Battle;
using HackmonInternals.Models;

namespace HackmonInternals.Events.Battle;

public class HackmonAttack : HackmonBattleEvent
{
    public HackmonMove Move;
    
    public HackmonAttack(BattleManager manager, HackmonInstance hackmon, HackmonMove move) : base(manager, hackmon)
    {
        Move = move;
    }
}