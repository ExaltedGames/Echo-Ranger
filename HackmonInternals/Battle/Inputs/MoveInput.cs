using HackmonInternals.Models;

namespace HackmonInternals.Battle.Inputs;

public class MoveInput : TurnInput
{
    public HackmonInstance User { get; set; } 
    public HackmonInstance Target { get; set; }
    public int MoveId { get; set; }
}