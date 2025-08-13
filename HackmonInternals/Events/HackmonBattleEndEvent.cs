namespace HackmonInternals.Events;

public class HackmonBattleEndEvent(bool playerWon) : HackmonBattleEvent
{
    public bool PlayerWin = playerWon;
}