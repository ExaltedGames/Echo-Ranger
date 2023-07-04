using HackmonInternals.Battle.Negotiators;
using HackmonInternals.Enums;
using HackmonInternals.Events.Battle;
using HackmonInternals.Models;

namespace HackmonInternals.Battle;

public class BattleManager
{
    public Queue<BattleEvent> EventQueue { get; }
    
    public InputNegotiator InputNegotiator { get; set; }

    public TrainerData EnemyData { get; }

    public TrainerData PlayerData { get; }

    public List<HackmonInstance> EnemyParty => EnemyData.CurrentParty;

    public List<HackmonInstance> PlayerParty => PlayerData.CurrentParty;

    public int? AliveEnemyHackmon => EnemyParty?.Count(h => !h.IsDead);

    public int? AlivePlayerHackmon => PlayerParty?.Count(h => !h.IsDead);

    public HackmonInstance CurrentPlayerMon { get; }
    public HackmonInstance CurrentEnemyMon { get; }

    public BattlePhase CurrentPhase;

    public BattleManager(TrainerData playerData, TrainerData enemyData, InputNegotiator? inputNegotiator = null)
    {
        EventQueue = new();
        PlayerData = playerData;
        EnemyData = enemyData;
        InputNegotiator = inputNegotiator ?? new TestNegotiator();
        CurrentPhase = (BattlePhase)1; // Select first phase sans unknown state
        CurrentPlayerMon = PlayerParty[0];
        CurrentEnemyMon = EnemyParty[0];
    }

    public virtual void ProgressPhase()
    {
        switch (CurrentPhase)
        {
            case BattlePhase.PreBattle:
                // Handle setup, make sure status/debuff/buff arrays are as they should be.
                PreBattle();
               break;
            case BattlePhase.PreTurn:
                // Honestly not sure
                break;
            case BattlePhase.TurnStart:
                // Handle debuffs/buffs ticking down
                StartTurn();
                break;
            case BattlePhase.WaitingForInputs:
                // wait for player input 
                if (!InputNegotiator.InputsReady)
                {
                    Console.WriteLine("Inputs aren't ready yet!");
                    return;
                }
                var inputs = InputNegotiator.GetAllInputs();
                Console.WriteLine(inputs);
                break;
            case BattlePhase.TurnCommencing:
                // frontend displays selected attacks, swaps would happen here.
                break;
            case BattlePhase.HandleOutcome:
                // play animations of attacks, show damage
                break;
            case BattlePhase.TurnEnd:
                // handle EOT ongoing effects, debuffs, etc
                EndTurn();
                break;
            case BattlePhase.PostTurn:
                // not sure honestly.
                break;
            case BattlePhase.PostBattle:
                // prize money, etc, victory/defeat screen
                return;
            case BattlePhase.Unknown:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected virtual void PreBattle()
    {
        EventQueue.Enqueue(new MonEnteredBattle(this, CurrentEnemyMon));
        EventQueue.Enqueue(new MonEnteredBattle(this, CurrentPlayerMon));

        CurrentPhase = BattlePhase.PreTurn;
    }

    protected virtual void StartTurn()
    {
        EventQueue.Enqueue(new TurnStart(this));
    }

    protected virtual void EndTurn()
    {
        EventQueue.Enqueue(new TurnEnd(this));
    }

    protected virtual void CastMove(int moveId, HackmonInstance source, HackmonInstance target)
    {
        EventQueue.Enqueue(new HackmonAttack(this, source, HackmonManager.MoveRegistry[moveId]));
    }
}