using HackmonInternals.Enums;
using HackmonInternals.StatusEffects;
using TurnBasedBattleSystem;

namespace HackmonInternals.Models;

public class HackmonInstance : IUnit
{
   public HackmonData staticData { get; private set; }

   public string Name => staticData.Name;
   
   public int Level { get; set; }
   
   public int Health { get; set; }
   
   public int Speed { get; set; }

   public bool IsDead => Health <= 0;

   public int Attack => staticData.Attack.BaseValue + (int)MathF.Round(staticData.Attack.GrowthPerLevel * Level);

   public int SpAttack => staticData.SpAttack.BaseValue + (int)MathF.Round(staticData.SpAttack.GrowthPerLevel * Level);

   public int Defense => staticData.Defense.BaseValue + (int) MathF.Round(staticData.Defense.GrowthPerLevel * Level);

   public int SpDefense => staticData.SpDefense.BaseValue + (int)MathF.Round(staticData.SpDefense.GrowthPerLevel * Level);

   public int MaxHp => staticData.MaxHp.BaseValue + (int)MathF.Round(staticData.MaxHp.GrowthPerLevel * Level) + 99;

   public int MaxStamina { get; set; } = 100;
   
   public int Stamina { get; set; }

   public HackmonType PrimaryType => staticData.PrimaryType;
   
   public HackmonType? SecondaryType => staticData.SecondaryType;

   public List<Status> StatusEffects { get; set; } = new();

   public List<int> KnownMoves { get; set; } = new();

   public HackmonInstance(HackmonData staticData, int level)
   {
      this.staticData = staticData;
      Level = level;

      Health = MaxHp;
      Stamina = MaxStamina;
      
      //TODO: remove this later when we have better methods of generating this
      KnownMoves = staticData.LearnableMoves;
   }

   public List<IStatus> Statuses { get; set; }
}