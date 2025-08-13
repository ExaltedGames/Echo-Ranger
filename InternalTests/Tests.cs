using HackmonInternals;
using HackmonInternals.Battle;
using HackmonInternals.Models;
using TurnBasedBattleSystem.Actions;

namespace InternalTests;

public static class Tests
{
   public static void TestMoveLoading()
   {
      HackmonManager.LoadAllData();
      
      Console.WriteLine($"Moves Loaded: {HackmonManager.MoveRegistry.Count}");
      Console.WriteLine($"Hackmon Loaded:  {HackmonManager.HackmonRegistry.Count}");

      foreach (var move in HackmonManager.MoveRegistry.Values)
      {
         Console.WriteLine($"{move.Name} inflicts {move.TargetStatusTypes.Count} statuses.");
      }
   }

   public static void TestBattle()
   {
      var playerMon = new HackmonInstance(HackmonManager.HackmonRegistry[1], 1);
      var enemyMon = new HackmonInstance(HackmonManager.HackmonRegistry[2], 1);

      var playerTeam = new List<HackmonInstance>() { playerMon };
      var enemyTeam = new List<HackmonInstance>() { enemyMon };
      
      HackmonBattleManager.StartBattle(playerTeam, enemyTeam);
      while (HackmonBattleManager.InBattle)
      {
         var playerAction = new AttackAction(
            playerMon,
            enemyMon,
            new AttackResolver(HackmonManager.MoveRegistry[playerMon.KnownMoves[0]])
         );

         HackmonBattleManager.HandleInput([playerAction]);
      } 
   }
}