using HackmonInternals.Models;

namespace HackmonInternals.StatusEffects;

public abstract class Status
{
   public abstract string Name { get; set; }
   
   public virtual void OnApply(HackmonData target)
   {
      
   }

   public virtual void DoTick(HackmonData target)
   {
      
   }
}