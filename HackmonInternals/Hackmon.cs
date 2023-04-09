using System.Text.Json;
using System.Text.Json.Serialization;
using HackmonInternals.StatusEffects;

namespace HackmonInternals;

public class Hackmon
{
    public HackmonType PrimaryType { get; set; }
    public HackmonType? SecondaryType { get; set; }
    public HackmonFamily Family { get; set; }
    public Stat MaxHp { get; set; }
    public Stat Attack { get; set; }
    public Stat SpAttack { get; set; }
    public Stat Defense { get; set; }
    public Stat SpDefense { get; set; }
    public List<string> LearnableMoves { get; set; } 
    public List<string> KnownMoves { get; set; }
}