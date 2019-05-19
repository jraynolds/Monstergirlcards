using UnityEngine;
using System.Collections;

public class Effect
{
    public ICharacter caster;
    public ICharacter target;

    public int duration;

    public Effect()
    {

    }

    public void InvokeEffect(Player p1, Player p2 = null, CardLogic card1 = null, CardLogic card2 = null)
    {

    }

    public void InvokeEffect(Player p1, Player p2 = null, CreatureLogic card1 = null, CardLogic card2 = null)
    {

    }

}
