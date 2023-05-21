namespace HackmonInternals;

public static class BattleManager
{
    public static List<HackmonInstance> EnemyParty { get; set; }

    public static List<HackmonInstance> PlayerParty => PlayerData.CurrentParty;

    public static int CurrentEnemyHackmon = 0;

    public static int CurrentPlayerHackmon = 0;

    public static void OnTurnStart()
    {
    }

    public static void OnTurnEnd()
    {
        
    }

    public static void OnCastMove(bool isEnemy, int moveId)
    {
        
    }
}