using UnityEngine;
using System.Collections;

public class DealDamageCommand : Command {

    private int targetID;
    private int amount;
    private int healthAfter;

    public DealDamageCommand( int targetID, int amount, int healthAfter)
    {
        this.targetID = targetID;
        this.amount = amount;
        this.healthAfter = healthAfter;
    }

    public override void StartCommandExecution()
    {
        DebugManager.Instance.DebugMessage("Executing damage command.", DebugManager.MessageType.Commands);

        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        if (targetID == GlobalSettings.Instance.BottomPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
        {
            // target is a hero
            target.GetComponent<AvatarVisual>().TakeDamage(amount,healthAfter);
        }
        else
        {
            // target is a creature
            target.GetComponent<OneCreatureManager>().TakeDamage(amount, healthAfter);
        }
        CommandExecutionComplete();
    }
}
