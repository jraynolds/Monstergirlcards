using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Command 
{
    private Player p;
    private int DeadCreatureID;

    public CreatureDieCommand(int CreatureID, Player p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }

    public override void StartCommandExecution()
    {
        DebugManager.Instance.DebugMessage("Executing destroy command.", DebugManager.MessageType.Commands);

        p.PArea.tableVisual.RemoveCreatureWithID(DeadCreatureID);
    }
}
