using UnityEngine;
using System.Collections;

public class DamageAllAllies : SpellEffect
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = TurnManager.Instance.WhoseTurn.table.monstersOnTable.ToArray();
        // Check for hexproof
        // Check for hex bypassing
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            cl.Health -= specialAmount;
        }

        Player ally = TurnManager.Instance.WhoseTurn;
        new DealDamageCommand(ally.ID, specialAmount, healthAfter: ally.Health - specialAmount).AddToQueue();
        ally.Health -= specialAmount;
    }
}
