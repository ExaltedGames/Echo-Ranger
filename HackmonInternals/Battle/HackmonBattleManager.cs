using HackmonInternals.Events;
using HackmonInternals.Models;
using HackmonInternals.StatusEffects;
using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Actions;
using TurnBasedBattleSystem.Events;

namespace HackmonInternals.Battle;

public static class HackmonBattleManager
{
    public static bool InBattle => BattleManager.BattleInProgress;
    public static int TurnNum = 0;
    public static readonly Queue<HackmonBattleEvent> EventQueue = new(); 
    
    private static bool _endBattle = false;
    
    public static void StartBattle(List<HackmonInstance> playerTeam, List<HackmonInstance> enemyTeam)
    {
        EventQueue.Clear();
		BattleManager.Cleanup();
        BattleManager.OnDeath += BattleEndCheck;
        BattleManager.OnHit += HitLogger;
        BattleManager.OnGainStatus += LogStatus;
        BattleManager.OnTurnStart += LogTurnBoundary;
        BattleManager.OnTurnEnd += LogTurnBoundary;
        BattleManager.EnemyAI = new HackmonAi();
        BattleManager.StartBattle(playerTeam.Cast<IUnit>().ToList(), enemyTeam.Cast<IUnit>().ToList());
    }

    public static void HandleInput(List<BattleAction> a)
    {
        BattleManager.HandlePlayerInput(a);
    }

    private static void LogStatus(GainStatusEvent s)
    {
        var unit = (HackmonInstance)s.Unit;
        var status = (Status)s.Status;
        var stacks = s.Stacks;
        
        var hitEvent = new HackmonStatusEvent(unit, status, stacks);
        EventQueue.Enqueue(hitEvent);
        
        Console.WriteLine($"eventqueue now contains {EventQueue.Count} items");

        Console.WriteLine($"{unit.Name} gained {stacks} stacks of {status.Name}");
    }

    private static void LogTurnBoundary(BattleEvent b)
    {
        if (b is StartTurnEvent)
        {
            Console.WriteLine("Start of turn");
        }
        else if (b is EndTurnEvent)
        {
            Console.WriteLine("End of turn");
            var e = new HackmonEndTurnEvent();
            EventQueue.Enqueue(e);
            if (_endBattle)
            {
                _endBattle = false;
                BattleManager.Cleanup();
            }
        }
    }

    private static void HitLogger(HitEvent e)
    {
        var attacker = (HackmonInstance)e.Attacker;
        var target = (HackmonInstance)e.Target;
        var atk = (AttackResolver)e.Attack;

        var hitEvent = new HackmonHitEvent(attacker, target, atk.AttackData, e.Damage);
        EventQueue.Enqueue(hitEvent);
        
        Console.WriteLine($"EventQueue now contains {EventQueue.Count} items");
        
        Console.WriteLine(
            $"{attacker.Name} uses {atk.AttackData.Name} for {atk.AttackData.StaminaCost} stamina on {target.Name}\nDamage: {e.Damage}. {target.Name} HP Remaining: {target.Health}");
    }

    private static void BattleEndCheck(DeathEvent data)
    {
        // log death
        var deadUnit = (HackmonInstance)data.Unit;
        var deathEvent = new HackmonDeathEvent(deadUnit);
        EventQueue.Enqueue(deathEvent);
        
        Console.WriteLine($"{deadUnit.Name} has fainted!");

        var playerAlive = false;
        foreach (HackmonInstance u in BattleManager.PlayerTeam)
        {
            if (u.Health > 0)
            {
                playerAlive = true;
                break;
            }
        }

        if (!playerAlive)
        {
            Console.WriteLine("Battle ends in player loss.");
            _endBattle = true;
            var endEvent = new HackmonBattleEndEvent(false);
            EventQueue.Enqueue(endEvent);
            return;
        }

        var enemyAlive = BattleManager.AITeam.Cast<HackmonInstance>().Any(u => u.Health > 0);

        if (!enemyAlive)
        {
            Console.WriteLine("Battle ends in player victory.");
            var endEvent = new HackmonBattleEndEvent(true);
            EventQueue.Enqueue(endEvent);
            _endBattle = true;
        }
    }
}