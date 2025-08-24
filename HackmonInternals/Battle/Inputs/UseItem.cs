namespace HackmonInternals.Battle.Inputs;

public class UseItem : TurnInput
{
	public HackmonInstance Target { get; set; }
	public int ItemId { get; set; }
}