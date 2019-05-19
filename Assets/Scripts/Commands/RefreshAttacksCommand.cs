using UnityEngine;
using System.Collections;

public class RefreshAttacksCommand : Command
{
    private CreatureLogic target;

    public RefreshAttacksCommand(ICharacter target)
    {
        this.target = (CreatureLogic) target;
    }

    public override void StartCommandExecution()
    {
        DebugManager.Instance.DebugMessage("Executing attack refresh command.", DebugManager.MessageType.Commands);

        target.AttacksLeftThisTurn = target.attacksForOneTurn;

        CommandExecutionComplete();
    }
}
