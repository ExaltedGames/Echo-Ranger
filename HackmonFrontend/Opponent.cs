using Godot;
using HackmonFrontend;

public partial class Opponent : CharacterBody2D
{
	private void OnAreaEntered(PhysicsBody2D body)
	{
		if (body is Player player)
		{
			GD.Print("Player has entered ENGAGEMENT ZONE.");
			GameManager.Instance.LoadTestBattleData();
		}
	}
}
