using UnityEngine;
using System.Collections;

public class DestroyCommand : Command
{
    private CreatureLogic target;

    public DestroyCommand(ICharacter target)
    {
        this.target = (CreatureLogic) target;
    }

    public override void StartCommandExecution()
    {
        DebugManager.Instance.DebugMessage("Executing destroy command.", DebugManager.MessageType.Commands);

        target.Die();

        CommandExecutionComplete();
    }
}
