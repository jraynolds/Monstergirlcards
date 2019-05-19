using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour {

    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {
            // Check if the card is in a transition state
            bool transitioning = false;
            if (GetComponent<WhereIsTheCardOrCreature>() != null && GetComponent<WhereIsTheCardOrCreature>().VisualState == VisualStates.Transition) transitioning = true;
            if (GetComponentInParent<WhereIsTheCardOrCreature>() != null && GetComponentInParent<WhereIsTheCardOrCreature>().VisualState == VisualStates.Transition) transitioning = true;
            bool can = GlobalSettings.Instance.CanControlThisPlayer(PlayerOwner) && !transitioning;
            DebugManager.Instance.DebugMessage("Can drag? " + can, DebugManager.MessageType.Targeting);
            return can;
        }
    }

    protected virtual Player PlayerOwner
    {
        get{
            
            if (tag.Contains("Bottom"))
                return GlobalSettings.Instance.BottomPlayer;
            else if (tag.Contains("Top"))
                return GlobalSettings.Instance.TopPlayer;
            else
            {
                Debug.LogError("Untagged Card or creature " + transform.parent.name);
                return null;
            }
        }
    }

    protected abstract bool DragSuccessful();
}
