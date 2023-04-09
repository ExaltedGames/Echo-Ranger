using HackmonInternals;

namespace InternalTests;

public static class Tests
{
   public static void TestMoveLoading()
   {
      HackmonManager.LoadAllData();
      
      Console.WriteLine($"Moves Loaded: {HackmonManager.MoveRegistry.Count}");
      Console.WriteLine($"Hackmon Loaded:  {HackmonManager.HackmonRegistry.Count}");

      foreach (HackmonMove move in HackmonManager.MoveRegistry)
      {
         Console.WriteLine($"{move.Name} inflicts {move.TargetStatusList.Count} statuses.");
      }
   }
}