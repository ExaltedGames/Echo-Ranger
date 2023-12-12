using HackmonInternals.Models;
using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Actions;

namespace HackmonInternals.Battle;

public class HackmonAI : BattleAI
{
    public BattleAction DoAction(IUnit actor)
    {
        if (actor is HackmonInstance unit)
        {
            var moveId = unit.KnownMoves[0];
            var move = HackmonManager.MoveRegistry[moveId];
            var target = BattleManager.PlayerTeam[0];

            var action = new AttackAction(unit, target, new AttackResolver(move));

            return action;
        }

        throw new Exception("Used outside of intended context.");
    }
}