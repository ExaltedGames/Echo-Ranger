using HackmonInternals.Battle.Inputs;
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

    public HackmonInstance CurrentPlayerMon { get; private set; }
    public HackmonInstance CurrentEnemyMon { get; private set; }

    public BattlePhase CurrentPhase;

    public BattleManager(TrainerData playerData, TrainerData enemyData, InputNegotiator? inputNegotiator = null)
    {
        EventQueue = new();
        PlayerData = playerData;
        EnemyData = enemyData;
        InputNegotiator = inputNegotiator ?? new TestNegotiator();
        CurrentPhase = (BattlePhase) 1; // Select first phase sans unknown state
        CurrentPlayerMon = PlayerParty[0];
        CurrentEnemyMon = EnemyParty[0];
    }

    public virtual void ProgressPhase()
    {
        switch (CurrentPhase)
        {
            case BattlePhase.PreBattle:
                // Handle setup, make sure status/debuff/buff arrays are as they should be.
                CurrentPhase = PreBattle();
                break;
            case BattlePhase.PreTurn:
                // Honestly not sure
                break;
            case BattlePhase.TurnStart:
                // Handle debuffs/buffs ticking down
                CurrentPhase = StartTurn();
                break;
            case BattlePhase.WaitingForInputs:
                // wait for player input 

                break;
            case BattlePhase.TurnCommencing:
                // frontend displays selected attacks, swaps would happen here.
                break;
            case BattlePhase.HandleOutcome:
                // play animations of attacks, show damage
                break;
            case BattlePhase.TurnEnd:
                // handle EOT ongoing effects, debuffs, etc
                CurrentPhase = EndTurn();
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

    // Each phase handler is a single method that returns the next phase entered.

    protected virtual BattlePhase PreBattle()
    {
        EventQueue.Enqueue(new MonEnteredBattle(this, CurrentEnemyMon));
        EventQueue.Enqueue(new MonEnteredBattle(this, CurrentPlayerMon));

        return BattlePhase.TurnStart;
    }

    protected virtual BattlePhase StartTurn()
    {
        EventQueue.Enqueue(new TurnStart(this));

        return BattlePhase.WaitingForInputs;
    }

    protected virtual BattlePhase WaitForInputs()
    {
        if (!InputNegotiator.InputsReady)
        {
            Console.WriteLine("Inputs aren't ready yet!");
            return BattlePhase.WaitingForInputs;
        }

        var inputs = InputNegotiator.GetAllInputs();
        Console.WriteLine(inputs);

        return BattlePhase.TurnCommencing;
    }

    private List<BattleEvent> ResolveMove(HackmonInstance user, HackmonInstance target, int moveId)
    {
        if (!user.KnownMoves.Contains(moveId)) throw new Exception("Attempted to use a move not known to the user.");

        List<BattleEvent> events = new();
        HackmonMove usingMove = HackmonManager.MoveRegistry[moveId];

        // TODO: Resolve move, create a BattleEvent to represent each step in the resolution process, return events in order.
        // ((MovePower * Atk) + Level) / Def * STAB
        int attack;
        int defense;
        float stab = 1.0f;
        bool isStab = (user.staticData.PrimaryType == usingMove.MoveType);

        if (usingMove.AttackType is AttackType.Physical)
            {
            attack = user.Attack;
            defense = target.Defense;
            }
        else
            {
            attack = user.SpAttack;
            defense = target.SpDefense;
            }
        
        if (isStab)
            stab = 1.25f;
        
        var damage = ((usingMove.Damage * attack) + user.Level) / defense * stab;
       
        return events;
    }

    private List<BattleEvent> ResolveItem(HackmonInstance target, int itemID)
    {
        //TODO: implement items
        List<BattleEvent> events = new();

        return events;
    }
    

    protected virtual BattlePhase CommenceTurn()
    {
        if (!InputNegotiator.InputsReady)
        {
            throw new Exception("Invalid State Reached.");
        }

        var inputs = InputNegotiator.GetAllInputs();

        foreach (TurnInput input in inputs)
        {
            if (input is MoveInput move)
            {
                List<BattleEvent> battleEvents;
                if (CurrentPlayerMon == move.User)
                {
                    battleEvents = ResolveMove(CurrentPlayerMon, CurrentEnemyMon, move.MoveID);
                }
                else if (CurrentEnemyMon == move.User)
                {
                    battleEvents = ResolveMove(CurrentPlayerMon, CurrentEnemyMon, move.MoveID);
                }
                else throw new Exception("Invalid Move Input");

                foreach (BattleEvent e in battleEvents) EventQueue.Enqueue(e);
            }

            if (input is SwapMon swapMon)
            {
                if (CurrentPlayerMon == swapMon.SwapOut)
                {
                    EventQueue.Enqueue(new MonLeftBattle(this, CurrentPlayerMon));
                    CurrentPlayerMon = PlayerParty[swapMon.SwapIn];
                    EventQueue.Enqueue(new MonEnteredBattle(this, CurrentPlayerMon));
                }
                else if (CurrentEnemyMon == swapMon.SwapOut)
                {
                   EventQueue.Enqueue(new MonLeftBattle(this, CurrentEnemyMon));
                   CurrentEnemyMon = EnemyParty[swapMon.SwapIn];
                   EventQueue.Enqueue(new MonEnteredBattle(this, CurrentEnemyMon));
                }
                else throw new Exception("Invalid Swap Input");
            }

            if (input is UseItem item)
            {
                var events = ResolveItem(item.Target, item.ItemID);
                
                foreach(BattleEvent e in events) EventQueue.Enqueue(e);
            }
        }

        return BattlePhase.TurnEnd;
    }

    protected virtual BattlePhase EndTurn()
    {
        EventQueue.Enqueue(new TurnEnd(this));

        return BattlePhase.TurnStart;
    }

    protected virtual void CastMove(int moveId, HackmonInstance source, HackmonInstance target)
    {
        EventQueue.Enqueue(new HackmonAttack(this, source, HackmonManager.MoveRegistry[moveId]));
    }
}