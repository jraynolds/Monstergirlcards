using UnityEngine;
using System.Collections;
using System;

public class DivideStatsOfTarget : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic creature = (CreatureLogic)target;

        double divisor = specialAmount;
        int newHealth = (int)Math.Ceiling((double)(creature.Health / specialAmount));
        int newAttack = (int)Math.Ceiling((double)(creature.Attack / specialAmount));
        new DivideStatsCommand(creature.ID, newHealth, newAttack).AddToQueue();
        creature.Health = newHealth;
        creature.Attack = newAttack;
    }
}
