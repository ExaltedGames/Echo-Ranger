using System.Collections.Generic;
using Godot;
using HackmonInternals;
using HackmonInternals.Battle;
using HackmonInternals.Events;
using HackmonInternals.Models;
using TurnBasedBattleSystem.Actions;

namespace HackmonFrontend;

public partial class Battle : Node2D
{
	private Textbox eventText;
	private BattlerUI trainerUI;
	private BattlerUI enemyUI;
	private BattlerStage trainerStage;
	private BattlerStage enemyStage;
	private ActionSelectUI actionSelect;

	private HackmonInstance ActivePlayerMon;
	private HackmonInstance ActiveEnemyMon;
	private bool processEvents = false;
	private bool itsSoOver = false;
	private List<string> messageList = new();

	private void OnPlayerInput(HackmonMove move)
	{
		var action = new AttackAction(ActivePlayerMon, ActiveEnemyMon, new AttackResolver(move));

		HackmonBattleManager.HandleInput(new() { action });
		processEvents = true;
	}

	private void OnMessagesDone()
	{
		eventText.Disable();
		if (itsSoOver) return;
		actionSelect.SetEnabled(true);
	}

	public override void _Ready()
	{
		GetNode<Camera2D>("Camera").MakeCurrent();
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
		// reset potential old state
		processEvents = false;
		itsSoOver = false;
		messageList = new();
		eventText.Disable();
		
		HackmonBattleManager.StartBattle(playerData.CurrentParty, enemy.CurrentParty);

		ActivePlayerMon = playerData.CurrentParty[0];
		ActiveEnemyMon = enemy.CurrentParty[0];

		GD.Print($"Player sends out {ActivePlayerMon.Name}");
		GD.Print($"Enemy sends out: {enemy.CurrentParty[0].Name}");

		trainerUI.SetCurrentMon(ActivePlayerMon);
		trainerStage.LoadHackmon(ActivePlayerMon.Name, true);
		enemyUI.SetCurrentMon(enemy.CurrentParty[0]);
		enemyStage.LoadHackmon(enemy.CurrentParty[0].Name);

		HackmonMove[] battleMoveset = new HackmonMove[4];
		for (int i = 0; i < 4; i++)
		{
			if (ActivePlayerMon.KnownMoves.Count > i)
			{
				battleMoveset[i] = HackmonManager.MoveRegistry[ActivePlayerMon.KnownMoves[i]];
			}
			else break;
		}

		actionSelect.SetActions(battleMoveset);
		actionSelect.SetEnabled(true);
		actionSelect.ResetHandler();
		actionSelect.OnActionSelected += OnPlayerInput;
	}

	public override void _Process(double delta)
	{
		if (processEvents)
		{
			HackmonBattleEvent @event;
			while (HackmonBattleManager.EventQueue.TryDequeue(out @event))
			{
				var eventStr = "";
				switch (@event)
				{
					case HackmonEndTurnEvent:
						GD.Print($"attempting to display messages");
						actionSelect.SetEnabled(false);
						eventText.Enable();
						eventText.ShowMessages(new List<string>(messageList), OnMessagesDone);
						messageList.Clear();
						processEvents = false;
						break;
					case HackmonHitEvent hitEvent:
						GD.Print("adding message.");
						eventStr =
							$"{hitEvent.Attacker.Name} uses {hitEvent.Attack.Name} on {hitEvent.Target.Name} for {hitEvent.Damage} damage.";
						messageList.Add(eventStr);
						if (ActivePlayerMon == hitEvent.Attacker)
						{
							enemyUI.DoDamageAnim(hitEvent.Damage);    
						}
						else
						{
							trainerUI.DoDamageAnim(hitEvent.Damage);
						}
						break;
					case HackmonDeathEvent deathEvent:
						eventStr = $"{deathEvent.Unit.Name} has fainted.";
						messageList.Add(eventStr);
						break;
					case HackmonBattleEndEvent endEvent:
						eventStr = $"Battle ends in player {(endEvent.PlayerWin ? "victory" : "defeat")}";
						messageList.Add(eventStr);
						itsSoOver = true;
						
						// TODO: Two battle events are sent (presumably for each echo dying?) so this is dubious
						// Also this should get queued to only happen once the text is complete.
						GameManager.Instance.DeferredPopCurrentScene();
						break;
				}
			}
		}
	}
}
