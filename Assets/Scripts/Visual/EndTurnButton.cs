using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour {

    public void OnClick()
    {
        // disable the button so it can't be clicked multiple times by accident
        DebugManager.Instance.DebugMessage("Turn ended.", DebugManager.MessageType.Game);
        GetComponent<Button>().interactable = false;
        new EndATurnCommand().AddToQueue();
    }

}
