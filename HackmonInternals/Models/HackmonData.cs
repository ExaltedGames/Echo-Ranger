using HackmonInternals.Enums;

namespace HackmonInternals.Models;

public class HackmonData
{
    public HackmonType PrimaryType { get; set; }
    public HackmonType? SecondaryType { get; set; }
    public HackmonFamily Family { get; set; }
    public Stat MaxHp { get; set; }
    public Stat Attack { get; set; }
    public Stat SpAttack { get; set; }
    public Stat Defense { get; set; }
    public Stat SpDefense { get; set; }
    public List<int> LearnableMoves { get; set; } 
}