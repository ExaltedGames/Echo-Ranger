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

   public int Attack => StaticData.Attack.BaseValue + (int)MathF.Round(StaticData.Attack.GrowthPerLevel * Level);

   public int SpAttack => StaticData.SpAttack.BaseValue + (int)MathF.Round(StaticData.SpAttack.GrowthPerLevel * Level);

   public int Defense => StaticData.Defense.BaseValue + (int) MathF.Round(StaticData.Defense.GrowthPerLevel * Level);

   public int SpDefense => StaticData.SpDefense.BaseValue + (int)MathF.Round(StaticData.SpDefense.GrowthPerLevel * Level);

   public int MaxHp => StaticData.MaxHp.BaseValue + (int)MathF.Round(StaticData.MaxHp.GrowthPerLevel * Level) + 99;

   public int MaxStamina => StaticData.MaxStamina.BaseValue + (int)MathF.Round(StaticData.MaxStamina.GrowthPerLevel * Level) + 99;
   
   public int Stamina { get; set; }

   public HackmonType PrimaryType => StaticData.PrimaryType;
   
   public HackmonType? SecondaryType => StaticData.SecondaryType;

   //public List<Status> StatusEffects { get; set; } = new();

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

   public List<IStatus> Statuses { get; set; } = new();

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