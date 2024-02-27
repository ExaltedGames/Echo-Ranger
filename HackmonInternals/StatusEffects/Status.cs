using HackmonInternals.Models;
using TurnBasedBattleSystem;

namespace HackmonInternals.StatusEffects;

public abstract class Status : IStatus
{
   public int RemainingTurns;
   public string Name { get; set; }
   public HackmonInstance Unit { get; set; }
   public abstract void Remove(int stacks);

   protected Status(HackmonInstance unit, int numTurns)
   {
      Unit = unit;
      RemainingTurns = numTurns;
   }

}