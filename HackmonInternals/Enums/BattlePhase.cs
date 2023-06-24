namespace HackmonInternals.Enums;

public enum BattlePhase
{
    Unknown,
    PreBattle,
    PreTurn,
    TurnStart,
    WaitingForInputs,
    TurnCommencing,
    HandleOutcome, /* Handle death swaps */
    TurnEnd,
    PostTurn,
    PostBattle,
}