namespace HackmonInternals;

public abstract class HackmonMove
{
   public HackmonType MoveType { get; set; } 
   public string Name { get; set; }

   public abstract void OnUse(Hackmon user, Hackmon target);
}