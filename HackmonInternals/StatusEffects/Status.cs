using HackmonInternals.Models;

namespace HackmonInternals.StatusEffects;

public abstract class Status
{
   public abstract string Name { get; set; }
   public int RemainingTurns;

   protected Status(int numTurns)
   {
      RemainingTurns = numTurns;
   }

   public virtual void OnApply(HackmonInstance target)
   {
      
   }

   public virtual void DoTick(HackmonInstance target)
   {
      
   }
}