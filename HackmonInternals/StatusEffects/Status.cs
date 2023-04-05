namespace HackmonInternals.StatusEffects;

public abstract class Status
{
   public abstract string Name { get; set; }
   
   public virtual void OnApply(Hackmon target)
   {
      
   }

   public virtual void DoTick(Hackmon target)
   {
      
   }
}