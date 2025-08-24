using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Actions;

namespace HackmonInternals.Battle;

public class HackmonAI : IBattleAI
{
	public IBattleAction DoAction(IUnit actor)
	{
		if (actor is not HackmonInstance unit)
			throw new Exception("Used outside of intended context.");

		Random rand = new();
		var maxMoves = 4;
		if (unit.KnownMoves.Count < 4)
			maxMoves = unit.KnownMoves.Count;

		var choice = rand.Next(maxMoves);
		Console.WriteLine($"Selected {choice} from {maxMoves} total moves.");
		var moveId = unit.KnownMoves[choice];
		var move = HackmonManager.MoveRegistry[moveId];
		var target = BattleManager.PlayerTeam[0];

		var action = new AttackAction(unit, target, new AttackResolver(move));

		return action;
	}
}