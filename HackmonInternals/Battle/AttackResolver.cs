using HackmonInternals.Enums;
using HackmonInternals.Models;
using TurnBasedBattleSystem;
using TurnBasedBattleSystem.Events;

namespace HackmonInternals.Battle;

public class AttackResolver : IAttack
{
    public HackmonMove AttackData { get; private set; }

    public AttackResolver(HackmonMove atkData)
    {
        AttackData = atkData;
    }

    public IEnumerable<BattleEvent> Resolve(IUnit attacker, IUnit target)
    {
        //calc damage
        if (attacker is not HackmonInstance || target is not HackmonInstance)
        {
            throw new Exception("Use in unsupported scenario.");
        }

        HackmonInstance moveUser = (HackmonInstance)attacker;
        HackmonInstance moveTarget = (HackmonInstance)target;
        if (AttackData.StaminaCost <= moveUser.Stamina)
        {
            moveUser.Stamina -= AttackData.StaminaCost;
        }
        else
        {
            throw new Exception($"Not enough stamina for move: {AttackData.Name}");
        }
        
        if (AttackData.Damage != 0)
        {
            var atk = 0;
            var def = 0;
            var stab = (moveUser.PrimaryType == AttackData.MoveType) ? 1.20f : 1f;
            var elements = HackmonManager.ElementInteractionsRegistry;
            
            float elementModifier = 1.0f;
            if (elements != null)
            {
                elementModifier *= elements[AttackData.MoveType][moveTarget.PrimaryType];
                elementModifier *= (moveTarget.SecondaryType != null) ?
                    elements[AttackData.MoveType][moveTarget.SecondaryType.Value] : 1.0f;
            }

            switch (AttackData.AttackType)
            {
                case AttackType.None:
                    throw new Exception(
                        "If move does damage, AttackType must be one of either 'Physical' or 'Special'");
                    break;
                case AttackType.Physical:
                    atk = moveUser.Attack;
                    def = moveTarget.Defense;
                    break;
                case AttackType.Special:
                    atk = moveUser.SpAttack;
                    def = moveTarget.SpDefense;
                    break;
            }
            
            int damage =  Math.Max(1, (int)((atk / ((def+100)/(100 * elementModifier)) + (AttackData.Damage * stab) - (moveTarget.Level/3)) * elementModifier));

            moveTarget.Health -= damage;
            HitEvent damageEvent = new HitEvent(moveUser, moveTarget, this, damage);
            yield return damageEvent;
        }

        if (AttackData.TargetStatuses.Count > 0)
        {
            foreach (var status in AttackData.TargetStatuses)
            {
                var statusName = status.Name;
                var duration = status.Duration;
                var statusInstance = moveTarget.StatusEffects.Find(effect => effect.Name == statusName);
                if (statusInstance != null)
                {
                   statusInstance.Add(duration); 
                }
                else
                {
                    statusInstance = HackmonManager.InstanceStatus(statusName, (HackmonInstance)target, duration);
                }
                var sEvent = new GainStatusEvent(target, statusInstance, duration);

                yield return sEvent;
            }
        }

        if (AttackData.UserStatuses.Count > 0)
        {
            foreach (var status in AttackData.UserStatuses)
            {
                var statusName = status.Name;
                var duration = status.Duration;
                var statusInstance = moveTarget.StatusEffects.Find(effect => effect.Name == statusName);
                if (statusInstance != null)
                {
                    statusInstance.Add(duration);
                }
                else
                {
                    statusInstance = HackmonManager.InstanceStatus(statusName, (HackmonInstance)target, duration);
                }
                var sEvent = new GainStatusEvent(target, statusInstance, duration);

                yield return sEvent;
            }
        }
    }
}