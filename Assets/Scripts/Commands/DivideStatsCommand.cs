using UnityEngine;
using System.Collections;

public class DivideStatsCommand : Command
{
    private int targetID;
    private int healthAfter;
    private int attackAfter;

    public DivideStatsCommand(int targetID, int healthAfter, int attackAfter)
    {
        this.targetID = targetID;
        this.healthAfter = healthAfter;
        this.attackAfter = attackAfter;
    }

    public override void StartCommandExecution()
    {
        DebugManager.Instance.DebugMessage("Executing stat divide command.", DebugManager.MessageType.Commands);

        GameObject target = IDHolder.GetGameObjectWithID(targetID);
        target.GetComponent<OneCreatureManager>().ChangeStat("health", healthAfter);
        target.GetComponent<OneCreatureManager>().ChangeStat("attack", healthAfter);

        CommandExecutionComplete();
    }
}
