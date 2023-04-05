namespace HackmonInternals;

public class Stat
{
   public int Value { get; set; }
   public float GrowthPerLevel { get; set; }
   public int BaseValue { get; set; }
   public List<Modifier> Modifiers { get; set; } = new();
}