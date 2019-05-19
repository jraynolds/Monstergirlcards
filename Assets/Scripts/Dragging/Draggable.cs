using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// This class enables Drag and Drop Behaviour for the game object it is attached to. 
/// It uses other script - DraggingActions to determine whether we can drag this game object now or not and 
/// whether the drop was successful or not.
/// </summary>

public class Draggable : MonoBehaviour {

    // PRIVATE FIELDS

    // a flag to know if we are currently dragging this GameObject
    private bool dragging = false;

    // distance from the center of this Game Object to the point where we clicked to start dragging 
    private Vector3 pointerDisplacement;

    // distance from camera to mouse on Z axis 
    private float zDisplacement;

    // reference to DraggingActions script. Dragging Actions should be attached to the same GameObject.
    private DraggingActions da;

    // STATIC property that returns the instance of Draggable that is currently being dragged
    public static Draggable DraggingThis { get; private set; }

    // MONOBEHAVIOUR METHODS
    void Awake()
    {
        da = GetComponent<DraggingActions>();
    }

    void OnMouseDown()
    {
        if (da!=null && da.CanDrag)
        {
            DebugManager.Instance.DebugMessage("Beginning to drag.", DebugManager.MessageType.PlayerInteraction, gameObject);
            dragging = true;
            // when we are dragging something, all previews should be off
            HoverPreview.PreviewsAllowed = false;
            // All highlights should be turned off
            CardActionManager.Instance.UnHighlightAll();
            DraggingThis = this;
            da.OnStartDrag();
            zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            pointerDisplacement = -transform.position + MouseInWorldCoords();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (dragging)
        { 
            Vector3 mousePos = MouseInWorldCoords();
            transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);   
            da.OnDraggingInUpdate();
        }
    }
	
    void OnMouseUp()
    {
        if (dragging)
        {
            DebugManager.Instance.DebugMessage("Let go.", DebugManager.MessageType.PlayerInteraction);
            dragging = false;
            DraggingThis = null;
            da.OnEndDrag();
            
            // turn all previews back on
            HoverPreview.PreviewsAllowed = true;
            // unhighlight all we did
            CardActionManager.Instance.UnHighlightAll();
            // rehighlight all we want
            CardActionManager.Instance.HighlightCastableCards(TurnManager.Instance.WhoseTurn);
            CardActionManager.Instance.HighlightMonstersWithAttacks(TurnManager.Instance.WhoseTurn);
        }
    }   

    // returns mouse position in World coordinates for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        screenMousePos.z = zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
        
}
