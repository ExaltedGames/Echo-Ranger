using System.Text.Json.Serialization;
using HackmonInternals.Enums;
using HackmonInternals.StatusEffects;
using TurnBasedBattleSystem;

namespace HackmonInternals.Models;

public class HackmonInstance : IUnit
{
   [JsonInclude]
   public HackmonData StaticData { get; private set; }

   public string Species
   {
      get => StaticData.Name;
      set => setSpecies(value);
   }

   public string Name => Nickname ?? StaticData.Name;

   public string? Nickname = null;

   public string Coloration { get; set; } = "Default";
   
   public int Level { get; set; }

   public int Experience { get; set; } = 0;

   public int Loyalty { get; set; } = 0;
   
   public int Health { get; set; }
   
   public int Speed { get; set; }
   
   public bool IsDead => Health <= 0;

   public int Attack => computeStatValue(StatType.Attack, StaticData.Attack); 

   public int SpAttack => computeStatValue(StatType.SpAttack, StaticData.SpAttack); 

   public int Defense => computeStatValue(StatType.Defense, StaticData.Defense);

   public int SpDefense => computeStatValue(StatType.SpDefense, StaticData.SpDefense);

   public int MaxHp => computeStatValue(StatType.MaxHp, StaticData.MaxHp);

   public int MaxStamina { get; set; } = 100;
   
   public int Stamina { get; set; }

   [JsonIgnore] public Dictionary<StatType, List<Modifier>> StatModifiers = new()
   {
      { StatType.MaxHp, new List<Modifier>() { new Modifier() { BaseAdditiveBonus = 99 } } }
   };

   public HackmonType PrimaryType => StaticData.PrimaryType;
   
   public HackmonType? SecondaryType => StaticData.SecondaryType;

   public List<Status> StatusEffects { get; set; } = new();

   public List<int> KnownMoves { get; set; }

   public List<int> ActiveMoves { get; set; } = new();

   public HackmonInstance(HackmonData staticData, int level)
   {
      this.StaticData = staticData;
      Level = level;
      
      Health = MaxHp;
      Stamina = MaxStamina;
      
      //TODO: remove this later when we have better methods of generating this
      KnownMoves = staticData.LearnableMoves;
   }

   private int computeStatValue(StatType type, Stat baseStat)
   {
      var mods = StatModifiers[type];
      var baseAdditiveBonus = mods.Aggregate(0, (acc, x) => acc + x.BaseAdditiveBonus);
      var multiplicativeBonus = mods.Aggregate<Modifier, double>(1, (acc, x) => acc + x.Multiplier);
      var stat = (baseStat.BaseValue + baseAdditiveBonus + (baseStat.GrowthPerLevel * Level)) * multiplicativeBonus;
      return (int)Math.Round(stat);
   }

   private void setSpecies(string speciesName)
   {
      var newSpecies = HackmonManager.HackmonRegistry.Values.FirstOrDefault(data => data.Name == speciesName);
      Console.WriteLine($"TETJSETLKSJKJTSLKJTSL {speciesName} | {newSpecies == null}");
      if (newSpecies != null)
      {
         StaticData = newSpecies;
      }
   }
}