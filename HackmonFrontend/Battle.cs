using Godot;
using Hackmon.Debugging;
using HackmonInternals;
using HackmonInternals.Battle;
using HackmonInternals.Models;

namespace HackmonFrontend;

public partial class Battle : Node2D
{
	private Textbox eventText;
	private BattlerUI trainerUI;
	private BattlerUI enemyUI;
	private BattlerStage trainerStage;
	private BattlerStage enemyStage;
	private ActionSelectUI actionSelect;

	private void ShowMessage(string message)
	{
		actionSelect.SetEnabled(false);
		eventText.Enable();
		eventText.SetText(message);
	}
	
	public override void _Ready()
	{
		eventText = GetNode<Textbox>("UI/Textbox");
		trainerUI = GetNode<BattlerUI>("UI/BattlerUI");
		trainerStage = GetNode<BattlerStage>("BattlerStage");
		enemyStage = GetNode<BattlerStage>("OpponentStage");
		enemyUI = GetNode<BattlerUI>("UI/OpponentUI");
		actionSelect = GetNode<ActionSelectUI>("UI/ActionSelectUI");
		
		eventText.SetText("Test Hackmon Takes 40 damage from MISSINGNO :)");
		
		GD.Print("test thingy");
		
		//eventText.Disable();
	}

	// The auto-run persistent script will be in charge of passing in relevant data to this scene via this method,
	// such as the TrainerData for the player and opponent.
	public void InitBattle(TrainerData playerData, TrainerData enemy)
	{
		HackmonBattleManager.StartBattle(playerData.CurrentParty, enemy.CurrentParty);

		var playerStarter = playerData.CurrentParty[0];
		
		GD.Print($"Player sends out {playerStarter.Name}");
		GD.Print($"Enemy sends out: {enemy.CurrentParty[0].Name}");
		
		trainerUI.SetCurrentMon(playerStarter);
		trainerStage.LoadHackmon(playerStarter.Name, true);
		enemyUI.SetCurrentMon(enemy.CurrentParty[0]);
		enemyStage.LoadHackmon(enemy.CurrentParty[0].Name);

		HackmonMove[] battleMoveset = new HackmonMove[4];
		for (int i = 0; i < 4; i++)
		{
			if (playerStarter.KnownMoves.Count > i)
			{
				battleMoveset[i] = HackmonManager.MoveRegistry[playerStarter.KnownMoves[i]];
			}
			else break;
		}
		
		actionSelect.SetActions(battleMoveset);
		ShowMessage($"Go, {playerStarter.Name}!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// InitBattle should be getting called before any processing gets done, this should make sure of that.
		// (and also keep me from having to make the entirety of event handling null-safe)
		
		//TODO: set up animations and shit
		
			//TODO: handle battle events	
	}
}
