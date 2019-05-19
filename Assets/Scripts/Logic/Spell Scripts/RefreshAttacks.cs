using UnityEngine;
using System.Collections;

public class RefreshAttacks : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new RefreshAttacksCommand(target).AddToQueue();
    }
}
