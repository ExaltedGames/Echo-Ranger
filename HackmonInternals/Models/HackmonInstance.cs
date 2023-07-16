using HackmonInternals.StatusEffects;

namespace HackmonInternals.Models;

public class HackmonInstance
{
   public HackmonData staticData { get; private set; }
   public int Level { get; set; }
   
   public int Hp { get; set; }

   public bool IsDead => Hp <= 0;

   public int Attack => staticData.Attack.BaseValue + (int)MathF.Round(staticData.Attack.GrowthPerLevel * Level);

   public int SpAttack => staticData.SpAttack.BaseValue + (int)MathF.Round(staticData.SpAttack.GrowthPerLevel * Level);

   public int Defense => staticData.Defense.BaseValue + (int) MathF.Round(staticData.Defense.GrowthPerLevel * Level);

   public int SpDefense => staticData.SpDefense.BaseValue + (int)MathF.Round(staticData.SpDefense.GrowthPerLevel * Level);

   public int MaxHp => staticData.MaxHp.BaseValue + (int)MathF.Round(staticData.MaxHp.GrowthPerLevel * Level);

   public int MaxStamina { get; set; } = 100;
   
   public int Stamina { get; set; }

   public List<Status> StatusEffects { get; set; } = new();

   public List<int> KnownMoves { get; set; } = new();

   public HackmonInstance(HackmonData staticData, int level)
   {
      this.staticData = staticData;
      Level = level;

      Hp = MaxHp;
      Stamina = MaxStamina;
   }
}