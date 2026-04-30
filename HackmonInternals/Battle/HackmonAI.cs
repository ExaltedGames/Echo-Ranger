using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Actions;

namespace HackmonInternals.Battle;

public class HackmonAI : IBattleAI
{
	public IBattleAction DoAction(IUnit actor)
	{
		if (actor is not HackmonInstance unit)
			throw new Exception("Used outside of intended context.");

		var validMoves = unit.KnownMoves.Where(moveCandidate =>
		{
			var moveEquals = HackmonManager.MoveRegistry[moveCandidate];
			Console.WriteLine($"{moveCandidate} chosen that subtracts {moveEquals.StaminaCost} from unit's {unit.Stamina}");
			return moveEquals.StaminaCost <= unit.Stamina;
		}).ToList();
		
		Random rand = new();
		var maxMoves = 6;
		if (validMoves.Count < 6)
			maxMoves = validMoves.Count;

		var choice = rand.Next(maxMoves);
		Console.WriteLine($"Selected {choice} from {maxMoves} total moves.");
		var moveId = validMoves[choice];
		var move = HackmonManager.MoveRegistry[moveId];
		var target = BattleManager.PlayerTeam[0];

		var action = new AttackAction(unit, target, new AttackResolver(move));

		return action;
	}
}