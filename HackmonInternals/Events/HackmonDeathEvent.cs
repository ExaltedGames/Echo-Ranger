using HackmonInternals.Models;
using TurnBasedBattleSystem.Events;

namespace HackmonInternals.Events;

public class HackmonDeathEvent : HackmonBattleEvent
{
   public HackmonDeathEvent(HackmonInstance unit)
   {
      Unit = unit;
   }
   
   public HackmonInstance Unit { get; set; }
}