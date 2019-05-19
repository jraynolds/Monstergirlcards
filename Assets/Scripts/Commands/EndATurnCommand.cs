using UnityEngine;
using System.Collections;

public class EndATurnCommand : Command
{

    public EndATurnCommand()
    {

    }

    public override void StartCommandExecution()
    {
        TurnManager.Instance.EndTurn();
        CommandExecutionComplete();
    }
}
