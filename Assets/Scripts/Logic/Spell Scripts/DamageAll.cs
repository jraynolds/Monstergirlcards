using UnityEngine;
using System.Collections;

public class DamageAll : SpellEffect
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        CreatureLogic[] CreaturesToDamage = TurnManager.Instance.WhoseTurn.OtherPlayer.table.monstersOnTable.ToArray();
        // Check for hexproof
        // Check for hex bypassing
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            cl.Health -= specialAmount;
        }

        Player enemy = TurnManager.Instance.WhoseTurn.OtherPlayer;
        new DealDamageCommand(enemy.ID, specialAmount, healthAfter: enemy.Health - specialAmount).AddToQueue();
        enemy.Health -= specialAmount;

        CreaturesToDamage = TurnManager.Instance.WhoseTurn.table.monstersOnTable.ToArray();
        // Check for hexproof
        // Check for hex bypassing
        foreach (CreatureLogic cl in CreaturesToDamage)
        {
            new DealDamageCommand(cl.ID, specialAmount, healthAfter: cl.Health - specialAmount).AddToQueue();
            cl.Health -= specialAmount;
        }

        Player ally = TurnManager.Instance.WhoseTurn.OtherPlayer;
        new DealDamageCommand(ally.ID, specialAmount, healthAfter: ally.Health - specialAmount).AddToQueue();
        ally.Health -= specialAmount;
    }
}
