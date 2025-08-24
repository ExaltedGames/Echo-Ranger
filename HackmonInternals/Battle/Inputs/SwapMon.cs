namespace HackmonInternals.Battle.Inputs;

public class SwapMon : TurnInput
{
	public HackmonInstance SwapOut { get; set; }
	public int SwapIn { get; set; }
}