using UnityEngine;
using System.Collections;

public class DamageOpponentBattlecry : CreatureEffect
{
    public DamageOpponentBattlecry(Player owner, CreatureLogic creature, int specialAmount): base(owner, creature, specialAmount)
    {}

    // BATTLECRY
    public override void WhenACreatureIsPlayed()
    {
        new DealDamageCommand(owner.OtherPlayer.PlayerID, specialAmount, owner.OtherPlayer.Health - specialAmount).AddToQueue();
        owner.OtherPlayer.Health -= specialAmount;
    }
}
