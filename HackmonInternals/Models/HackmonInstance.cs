using System.Text.Json.Serialization;
using HackmonInternals.StatusEffects;
using JetBrains.Annotations;
using TurnBasedBattleSystem;

namespace HackmonInternals.Models;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public class HackmonInstance : IUnit
{
	[JsonInclude]
	public HackmonData StaticData { get; private set; }

	public string Species
	{
		get => StaticData.Name;
		set => SetSpecies(value);
	}

	public string Name => Nickname ?? StaticData.Name;

	public string Coloration { get; set; } = "Default";

	public int Level { get; set; }

	public int Experience { get; set; } = 0;

	public int Loyalty { get; set; } = 0;

	public bool IsDead => Health <= 0;

	public int Attack => ComputeStatValue(StatType.Attack, StaticData.Attack);

	public int SpAttack => ComputeStatValue(StatType.SpAttack, StaticData.SpAttack);

	public int Defense => ComputeStatValue(StatType.Defense, StaticData.Defense);

	public int SpDefense => ComputeStatValue(StatType.SpDefense, StaticData.SpDefense);

	public int MaxHp => ComputeStatValue(StatType.MaxHp, StaticData.MaxHp);

	public int MaxStamina =>
		StaticData.MaxStamina.BaseValue + (int)MathF.Round(StaticData.MaxStamina.GrowthPerLevel * Level) + 99;

	public int Stamina { get; set; }

	public HackmonType PrimaryType => StaticData.PrimaryType;

	public HackmonType? SecondaryType => StaticData.SecondaryType;

	public List<Status> StatusEffects { get; set; } = [];

	public List<int> KnownMoves { get; set; } = [];

	public List<int> ActiveMoves { get; set; } = [];
	public string? Nickname = null;

	[JsonIgnore]
	public Dictionary<StatType, List<Modifier>> StatModifiers = new()
	{
		{ StatType.Attack, [new Modifier { BaseAdditiveBonus = 99 }] },
		{ StatType.SpAttack, [new Modifier { BaseAdditiveBonus = 99 }] },
		{ StatType.MaxHp, [new Modifier { BaseAdditiveBonus = 99 }] },
		{ StatType.Defense, [new Modifier { BaseAdditiveBonus = 99 }] },
		{ StatType.SpDefense, [new Modifier { BaseAdditiveBonus = 99 }] }
	};

	public int Health { get; set; }

	public int Speed { get; set; }

	public List<IStatus> Statuses { get; set; } = [];

	public HackmonInstance(HackmonData staticData, int level)
	{
		StaticData = staticData;
		Level = level;

		Health = MaxHp;
		Stamina = MaxStamina;

		//Finds which moves are valid for current level, selects the move ID, and sends it to the KnownMoves list.
		KnownMoves = StaticData.LearnableMoves
			.Where(move => Level >= move[1])
			.Select(move => move[0])
			.ToList();
	}

	private int ComputeStatValue(StatType type, Stat baseStat)
	{
		var mods = StatModifiers[type];
		var baseAdditiveBonus = mods.Aggregate(0, (acc, x) => acc + x.BaseAdditiveBonus);
		var multiplicativeBonus = mods.Aggregate<Modifier, double>(1, (acc, x) => acc + x.Multiplier);
		var stat = (baseStat.BaseValue + baseAdditiveBonus + baseStat.GrowthPerLevel * Level) * multiplicativeBonus;
		return (int)Math.Round(stat);
	}

	private void SetSpecies(string speciesName)
	{
		var newSpecies = HackmonManager.HackmonRegistry.Values.FirstOrDefault(data => data.Name == speciesName);
		Console.WriteLine($"TETJSETLKSJKJTSLKJTSL {speciesName} | {newSpecies == null}");
		if (newSpecies != null)
			StaticData = newSpecies;
	}
}