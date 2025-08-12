using HackmonInternals.Models;

namespace HackmonInternals.Events;

public class HackmonDeathEvent(HackmonInstance unit) : HackmonBattleEvent
{
   public HackmonInstance Unit { get; set; } = unit;
}