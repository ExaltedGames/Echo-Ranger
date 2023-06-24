using HackmonInternals.Battle;
using HackmonInternals.Models;

namespace HackmonInternals.Events.Battle;

public class TakeDamage : HackmonBattleEvent
{
    public int Damage;
    public bool Critical;
    
    public TakeDamage(BattleManager manager, HackmonInstance hackmon, int damage, bool critical) : base(manager, hackmon)
    {
        Damage = damage;
        Critical = critical;
    }
}