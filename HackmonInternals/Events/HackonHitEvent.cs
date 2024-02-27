using HackmonInternals.Models;

namespace HackmonInternals.Events;

public class HackmonHitEvent : HackmonBattleEvent
{
    public HackmonHitEvent(HackmonInstance attacker, HackmonInstance target, HackmonMove attack, int dmg)
    {
        Attacker = attacker;
        Target = target;
        Attack = attack;
        Damage = dmg;
    }
    
    public HackmonInstance Attacker { get; set; }
    public HackmonInstance Target { get; set; }
    public HackmonMove Attack { get; set; }
    public int Damage { get; set; }
}