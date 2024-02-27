using HackmonInternals.Models;

namespace HackmonInternals.Battle.Inputs;

public class UseItem : TurnInput
{
   public HackmonInstance Target { get; set; } 
   public int ItemID { get; set; }
}