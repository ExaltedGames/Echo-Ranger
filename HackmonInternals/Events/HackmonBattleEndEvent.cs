namespace HackmonInternals.Events;

public class HackmonBattleEndEvent : HackmonBattleEvent
{
    public HackmonBattleEndEvent(bool playerWon)
    {
        PlayerWin = playerWon;
    }
    
    public bool PlayerWin;
}