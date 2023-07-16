using HackmonInternals;
using HackmonInternals.Battle;

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

      var battler = new BattleManager(new(), new());
      for (var i = 0; i < 20; i++)
      {
         battler.ProgressPhase();
         if (battler.EventQueue.TryDequeue(out var result))
         {
            Console.WriteLine($"Event: {result}");
         }
      }
   }
}