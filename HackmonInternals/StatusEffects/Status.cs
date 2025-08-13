using HackmonInternals.Models;
using JetBrains.Annotations;
using TurnBasedBattleSystem;

namespace HackmonInternals.StatusEffects;

[UsedImplicitly]
public abstract class Status(HackmonInstance unit, int stacks) : IStatus
{
   public int Stacks = stacks;
   public string Name { get; set; } = null!;
   public HackmonInstance Unit { get; set; } = unit;
   public abstract void Remove(int stacks);
   public abstract void Add(int stacks);
}