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

    public BattlePhase CurrentPhase;

    public BattleManager(TrainerData playerData, TrainerData enemyData, InputNegotiator? inputNegotiator = null)
    {
        EventQueue = new();
        PlayerData = playerData;
        EnemyData = enemyData;
        InputNegotiator = inputNegotiator ?? new TestNegotiator();
        CurrentPhase = (BattlePhase)1; // Select first phase sans unknown state
    }

    public virtual void ProgressPhase()
    {
        switch (CurrentPhase)
        {
            case BattlePhase.PreBattle:
                break;
            case BattlePhase.PreTurn:
                break;
            case BattlePhase.TurnStart:
                StartTurn();
                break;
            case BattlePhase.WaitingForInputs:
                if (!InputNegotiator.InputsReady)
                {
                    Console.WriteLine("Inputs aren't ready yet!");
                    return;
                }

                var inputs = InputNegotiator.GetAllInputs();
                Console.WriteLine(inputs);
                break;
            case BattlePhase.TurnCommencing:
                break;
            case BattlePhase.HandleOutcome:
                break;
            case BattlePhase.TurnEnd:
                EndTurn();
                break;
            case BattlePhase.PostTurn:
                break;
            case BattlePhase.PostBattle:
                return;
            
            case BattlePhase.Unknown:
            default:
                throw new ArgumentOutOfRangeException();
        }

        CurrentPhase++;
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