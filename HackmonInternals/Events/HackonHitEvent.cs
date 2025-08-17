namespace HackmonInternals.Events;

public class HackmonHitEvent(HackmonInstance attacker, HackmonInstance target, HackmonMove attack, int dmg)
	: HackmonBattleEvent
{
	public HackmonInstance Attacker { get; set; } = attacker;
	public HackmonInstance Target { get; set; } = target;
	public HackmonMove Attack { get; set; } = attack;
	public int Damage { get; set; } = dmg;
}