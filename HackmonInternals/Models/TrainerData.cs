namespace HackmonInternals.Models;

public class TrainerData
{
	public int Id { get; set; }
	public List<int[]> Party { get; set; } = [];
	public List<HackmonInstance> CurrentParty { get; set; } = [];
}