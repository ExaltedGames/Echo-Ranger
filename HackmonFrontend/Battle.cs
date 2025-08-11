using System;
using System.Threading.Tasks;
using Godot;
using HackmonInternals;
using HackmonInternals.Battle;
using HackmonInternals.Events;
using HackmonInternals.Models;
using TurnBasedBattleSystem.Actions;

namespace HackmonFrontend;

public partial class Battle : Node2D
{
	private Textbox _eventText;
	private BattlerUI _trainerUi;
	private BattlerUI _enemyUi;
	private BattlerStage _trainerStage;
	private BattlerStage _enemyStage;
	private ActionSelectUI _actionSelect;

	private HackmonInstance _activePlayerMon;
	private HackmonInstance _activeEnemyMon;
	private bool _processEvents = false;
	private bool _itsSoOver = false;
	private int _playerPreRegenStamina = 0;
	private int _playerPreRegenHealth = 0;
	private int _enemyPreRegenStamina = 0;
	private int _enemyPreRegenHealth = 0;

	private void OnPlayerInput(HackmonMove move)
	{
		if (move.StaminaCost > _activePlayerMon.Stamina)
		{
			_actionSelect.SetEnabled(false);
			_eventText.QueueMessage("Not enough stamina!");
			_eventText.ShowMessages(OnMessagesDone);
			GD.Print("Nostamina.");
			return;
		}
		var action = new AttackAction(_activePlayerMon, _activeEnemyMon, new AttackResolver(move));

		HackmonBattleManager.HandleInput(new() { action });
		_processEvents = true;
	}

	private void OnMessagesDone()
	{
		_eventText.Disable();
		if (_itsSoOver)
		{
			// Return to overworld
			GameManager.Instance.DeferredPopCurrentScene();
			return;
		}
		_actionSelect.SetEnabled(true);
	}
	private async void TurnEndEvent()
	{
		await Task.WhenAll(
			_trainerUi.DoStamRegenAnim(_activePlayerMon.Stamina - _playerPreRegenStamina),
			_enemyUi.DoStamRegenAnim(_activeEnemyMon.Stamina - _enemyPreRegenStamina)
		);
		OnMessagesDone();
	}

	public override void _Ready()
	{
		GetNode<Camera2D>("Camera").MakeCurrent();
		_eventText = GetNode<Textbox>("UI/Textbox");
		_trainerUi = GetNode<BattlerUI>("UI/BattlerUI");
		_trainerStage = GetNode<BattlerStage>("BattlerStage");
		_enemyStage = GetNode<BattlerStage>("OpponentStage");
		_enemyUi = GetNode<BattlerUI>("UI/OpponentUI");
		_actionSelect = GetNode<ActionSelectUI>("UI/ActionSelectUI");

		_eventText.SetText("Test Hackmon Takes 40 damage from MISSINGNO :)");

		GD.Print("test thingy");

		//eventText.Disable();
	}

	// The auto-run persistent script will be in charge of passing in relevant data to this scene via this method,
	// such as the TrainerData for the player and opponent.
	public void InitBattle(TrainerData playerData, TrainerData enemy)
	{
		// reset potential old state
		_processEvents = false;
		_itsSoOver = false;
		_eventText.Disable();
		
		HackmonBattleManager.StartBattle(playerData.CurrentParty, enemy.CurrentParty);

		_activePlayerMon = playerData.CurrentParty[0];
		_activeEnemyMon = enemy.CurrentParty[0];

		GD.Print($"Player sends out {_activePlayerMon.Name}");
		GD.Print($"Enemy sends out: {enemy.CurrentParty[0].Name}");

		_trainerUi.SetCurrentMon(_activePlayerMon);
		_trainerStage.LoadHackmon(_activePlayerMon.Name, true);
		_enemyUi.SetCurrentMon(enemy.CurrentParty[0]);
		_enemyStage.LoadHackmon(enemy.CurrentParty[0].Name);

		HackmonMove[] battleMoveset = new HackmonMove[4];
		for (int i = 0; i < 4; i++)
		{
			if (_activePlayerMon.KnownMoves.Count > i)
			{
				battleMoveset[i] = HackmonManager.MoveRegistry[_activePlayerMon.KnownMoves[i]];
			}
			else break;
		}

		_actionSelect.SetActions(battleMoveset);
		_actionSelect.SetEnabled(true);
		_actionSelect.ResetHandler();
		_actionSelect.OnActionSelected += OnPlayerInput;
	}

	public override void _Process(double delta)
	{
		if (!_processEvents) return;
		HackmonBattleEvent @event;
		while (HackmonBattleManager.EventQueue.TryDequeue(out @event))
		{
			var eventStr = "";
			switch (@event)
			{
				case HackmonEndTurnEvent:
					GD.Print($"attempting to display messages");
					_actionSelect.SetEnabled(false);
					_eventText.Enable();
					_processEvents = false;
					if (_activePlayerMon.Stamina < _activePlayerMon.MaxStamina)
					{
						_playerPreRegenStamina = _activePlayerMon.Stamina;
						_activePlayerMon.Stamina += _activePlayerMon.MaxStamina / 8;
						_activePlayerMon.Stamina = Math.Min(_activePlayerMon.Stamina, _activePlayerMon.MaxStamina);
					}
					if (_activeEnemyMon.Stamina < _activeEnemyMon.MaxStamina)
					{
						_enemyPreRegenStamina = _activeEnemyMon.Stamina;
						_activeEnemyMon.Stamina += _activeEnemyMon.MaxStamina / 8;
						_activeEnemyMon.Stamina = Math.Min(_activeEnemyMon.Stamina, _activeEnemyMon.MaxStamina);
					}
					_eventText.ShowMessages(TurnEndEvent);
					break;
				case HackmonHitEvent hitEvent:
					GD.Print("adding message.");
					eventStr =
						$"{hitEvent.Attacker.Name} uses {hitEvent.Attack.Name} on {hitEvent.Target.Name} for {hitEvent.Damage} damage.";
					if (_activePlayerMon == hitEvent.Attacker)
					{
						_eventText.QueueMessage(eventStr, async () =>
						{
							await Task.WhenAll(
								_trainerUi.DoStaminaAnim(hitEvent.Attack.StaminaCost),
								_enemyUi.DoDamageAnim(hitEvent.Damage)
							);
						});
					}
					else
					{
						_eventText.QueueMessage(eventStr, async () =>
						{
							await Task.WhenAll(
								_enemyUi.DoStaminaAnim(hitEvent.Attack.StaminaCost),
								_trainerUi.DoDamageAnim(hitEvent.Damage)
							);
						});
					}
					break;
				case HackmonDeathEvent deathEvent:
					eventStr = $"{deathEvent.Unit.Name} has fainted.";
					_eventText.QueueMessage(eventStr);
					break;
				case HackmonBattleEndEvent endEvent:
					eventStr = $"Battle ends in player {(endEvent.PlayerWin ? "victory" : "defeat")}";
					_eventText.QueueMessage(eventStr);
					_itsSoOver = true;
					break;
			}
		}
	}
}
