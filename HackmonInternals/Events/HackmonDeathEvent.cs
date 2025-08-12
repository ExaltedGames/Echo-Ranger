using HackmonInternals.Models;

namespace HackmonInternals.Events;

public class HackmonDeathEvent : HackmonBattleEvent
{
   public HackmonDeathEvent(HackmonInstance unit)
   {
      Unit = unit;
   }
   
   public HackmonInstance Unit { get; set; }
}