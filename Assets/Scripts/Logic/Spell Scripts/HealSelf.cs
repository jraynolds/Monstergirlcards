using UnityEngine;
using System.Collections;

public class HealSelf : SpellEffect
{

    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        // Heal for an amount
        new DealDamageCommand(TurnManager.Instance.WhoseTurn.PlayerID, -specialAmount, TurnManager.Instance.WhoseTurn.Health + specialAmount).AddToQueue();
        TurnManager.Instance.WhoseTurn.Health += specialAmount;

    }
}
