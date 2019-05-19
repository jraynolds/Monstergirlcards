using UnityEngine;
using System.Collections;

public class DestroyTarget : SpellEffect
{
    public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
    {
        new DestroyCommand(target).AddToQueue();
    }
}
