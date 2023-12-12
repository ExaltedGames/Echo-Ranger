using HackmonInternals.Battle.Inputs;
using HackmonInternals.Battle.Negotiators;
using HackmonInternals.Enums;
using HackmonInternals.Models;
using HackmonInternals.StatusEffects;
using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Actions;
using TurnBasedBattleSystem.Events;

namespace HackmonInternals.Battle;

public static class HackmonBattleManager
{
    private static bool endBattle = false;
    
    public static void StartBattle(List<HackmonInstance> playerTeam, List<HackmonInstance> enemyTeam)
    {
        BattleManager.OnDeath += BattleEndCheck;
        BattleManager.OnHit += hitLogger;
        BattleManager.OnGainStatus += LogStatus;
        BattleManager.OnTurnStart += LogTurnBoundary;
        BattleManager.OnTurnEnd += LogTurnBoundary;
        BattleManager.EnemyAI = new HackmonAI();
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

        Console.WriteLine($"{unit.Name} gained {s.Stacks} stacks of {status.Name}");
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
            if (endBattle)
            {
                endBattle = false;
                BattleManager.Cleanup();
            }
        }
    }

    private static void hitLogger(HitEvent e)
    {
        HackmonInstance attacker = (HackmonInstance)e.Attacker;
        HackmonInstance target = (HackmonInstance)e.Target;
        AttackResolver atk = (AttackResolver)e.Attack;

        Console.WriteLine(
            $"{attacker.Name} uses {atk.AttackData.Name} on {target.Name}\nDamage: {e.Damage}. {target.Name} HP Remaining: {target.Health}");
    }

    private static void BattleEndCheck(DeathEvent data)
    {
        // log death
        HackmonInstance deadUnit = (HackmonInstance)data.Unit;
        Console.WriteLine($"{deadUnit.Name} has fainted!");

        bool playerAlive = false;
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
            endBattle = true;
            return;
        }

        bool enemyAlive = false;
        foreach (HackmonInstance u in BattleManager.AITeam)
        {
            if (u.Health > 0)
            {
                enemyAlive = true;
                break;
            }
        }

        if (!enemyAlive)
        {
            Console.WriteLine("Battle ends in player victory.");
            endBattle = true;
        }
    }
}