using HackmonFrontend.Game.Battle.UI;
using HackmonFrontend.Game.General;
using HackmonFrontend.Game.General.UI;
using HackmonInternals.Battle;
using HackmonInternals.Events;
using TurnBasedBattleSystem.Actions;

namespace HackmonFrontend.Game.Battle;

public partial class Battle : Node2D
{
	private BattlerStage _enemyStage = null!;
	private BattlerUI _enemyUi = null!;
	private BattlerStage _trainerStage = null!;
	private BattlerUI _trainerUi = null!;
	private ActionSelectUI _actionSelect = null!;

	private HackmonInstance? _activeEnemyMon;
	private HackmonInstance? _activePlayerMon;
	private int _enemyPreRegenHealth;
	private int _enemyPreRegenStamina;
	private Textbox? _eventText;
	private bool _itsSoOver;
	private int _playerPreRegenHealth;
	private int _playerPreRegenStamina;
	private bool _processEvents;

	public override void _Ready()
	{
		GetNodeOrNull<Camera2D>("Camera")?.MakeCurrent();
		_eventText = GetNodeOrNull<Textbox>("UI/Textbox");
		_trainerUi = GetNode<BattlerUI>("UI/BattlerUI");
		_trainerStage = GetNode<BattlerStage>("BattlerStage");
		_enemyStage = GetNode<BattlerStage>("OpponentStage");
		_enemyUi = GetNode<BattlerUI>("UI/OpponentUI");
		_actionSelect = GetNode<ActionSelectUI>("UI/ActionSelectUI");

		_eventText?.SetText("Test Hackmon Takes 40 damage from MISSINGNO :)");

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
		_eventText?.Disable();

		HackmonBattleManager.StartBattle(playerData.CurrentParty, enemy.CurrentParty);

		_activePlayerMon = playerData.CurrentParty[0];
		_activeEnemyMon = enemy.CurrentParty[0];

		GD.Print($"Player sends out {_activePlayerMon.Name}");
		GD.Print($"Enemy sends out: {enemy.CurrentParty[0].Name}");

		_trainerUi.SetCurrentMon(_activePlayerMon);
		_trainerStage.LoadHackmon(_activePlayerMon.Name, true);
		_enemyUi.SetCurrentMon(enemy.CurrentParty[0]);
		_enemyStage.LoadHackmon(enemy.CurrentParty[0].Name);

		var battleMoveset = new HackmonMove[6];
		for (var i = 0; i < 6; i++)
			if (_activePlayerMon.KnownMoves.Count > i)
				battleMoveset[i] = HackmonManager.MoveRegistry[_activePlayerMon.KnownMoves[i]];
			else
				break;

		_actionSelect.SetActions(battleMoveset);
		_actionSelect.SetEnabled(true);
		_actionSelect.ResetHandler();
		_actionSelect.OnActionSelected += OnPlayerInput;
	}

	public override void _Process(double delta)
	{
		if (!_processEvents || _activePlayerMon is null || _activeEnemyMon is null)
			return;

		while (HackmonBattleManager.EventQueue.TryDequeue(out var @event))
		{
			string eventStr;
			switch (@event)
			{
				case HackmonEndTurnEvent:
					GD.Print("attempting to display messages");
					_actionSelect.SetEnabled(false);
					_eventText?.Enable();
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

					_eventText?.ShowMessagesSync(TurnEndEvent);
					break;
				case HackmonHitEvent hitEvent:
					GD.Print("adding message.");
					eventStr =
						$"{hitEvent.Attacker.Name} uses {hitEvent.Attack.Name} on {hitEvent.Target.Name} for {hitEvent.Damage} damage.";

					_eventText?.QueueMessage(
						eventStr,
						async () =>
						{
							await Task.WhenAll(
								GetUiForUnit(hitEvent.Attacker).DoStaminaAnim(hitEvent.Attack.StaminaCost),
								GetUiForUnit(hitEvent.Attacker, true).DoDamageAnim(hitEvent.Damage)
							);
						}
					);

					break;

				// TODO This assumes that status' are only added and never expire
				case HackmonStatusEvent statusEvent:
					GD.Print("adding message.");
					eventStr =
						$"{statusEvent.Unit.Name} is afflicted with {statusEvent.Stacks} stacks of {statusEvent.Status.Name}.";

					_eventText?.QueueMessage(
						eventStr,
						() => GetUiForUnit(statusEvent.Unit).AddAilment(statusEvent.Status)
					);

					break;
				case HackmonDeathEvent deathEvent:
					eventStr = $"{deathEvent.Unit.Name} has fainted.";
					_eventText?.QueueMessage(eventStr);
					break;
				case HackmonBattleEndEvent endEvent:
					eventStr = $"Battle ends in player {(endEvent.PlayerWin ? "victory" : "defeat")}";
					_eventText?.QueueMessage(eventStr);
					_itsSoOver = true;
					break;
			}
		}
	}

	private void OnPlayerInput(HackmonMove move)
	{
		if (_activePlayerMon is null || _activeEnemyMon is null)
			return;

		if (move.StaminaCost > _activePlayerMon.Stamina)
		{
			_actionSelect.SetEnabled(false);
			_eventText?.QueueMessage("Not enough stamina!");
			_eventText?.ShowMessagesSync(OnMessagesDone);
			GD.Print("Nostamina.");
			return;
		}

		var action = new AttackAction(_activePlayerMon, _activeEnemyMon, new AttackResolver(move));

		HackmonBattleManager.HandleInput([action]);
		_processEvents = true;
	}

	private Task OnMessagesDone()
	{
		_eventText?.Disable();
		if (_itsSoOver)
		{
			// Return to overworld
			GameManager.Instance.DeferredPopCurrentScene();
			return Task.CompletedTask;
		}

		_actionSelect.SetEnabled(true);
		return Task.CompletedTask;
	}

	private async Task TurnEndEvent()
	{
		await Task.WhenAll(
			_trainerUi.DoStamRegenAnim(_activePlayerMon!.Stamina - _playerPreRegenStamina),
			_enemyUi.DoStamRegenAnim(_activeEnemyMon!.Stamina - _enemyPreRegenStamina)
		);

		await OnMessagesDone();
	}

	private BattlerUI GetUiForUnit(HackmonInstance unit, bool inverse = false) =>
		(unit == _activePlayerMon) ^ inverse ? _trainerUi : _enemyUi;
}