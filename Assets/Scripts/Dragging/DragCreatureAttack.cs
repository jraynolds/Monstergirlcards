using UnityEngine;
using System.Collections;
using System.Linq;

public class DragCreatureAttack : DraggingActions {

    // reference to the sprite with a round "Target" graphic
    private SpriteRenderer sr;
    // LineRenderer that is attached to a child game object to draw the arrow
    private LineRenderer lr;
    // reference to WhereIsTheCardOrCreature to track this object`s state in the game
    private WhereIsTheCardOrCreature whereIsThisCreature;
    // the pointy end of the arrow, should be called "Triangle" in the Hierarchy
    private Transform triangle;
    // SpriteRenderer of triangle. We need this to disable the pointy end if the target is too close.
    private SpriteRenderer triangleSR;
    // when we stop dragging, the gameObject that we were targeting will be stored in this variable.
    private GameObject Target;
    // Reference to creature manager, attached to the parent game object
    private OneCreatureManager manager;
    // Acceptable attack targets
    private TargetingOptions[] targets = new TargetingOptions[2] { TargetingOptions.EnemyCreatures, TargetingOptions.Enemy };

    void Awake()
    {
        // establish all the connections
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        lr.sortingLayerName = "Above Everything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();

        manager = GetComponentInParent<OneCreatureManager>();
        whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
            // we can drag this card if 
            // a) we can control this our player (this is checked in base.canDrag)
            // b) creature "CanAttackNow" - this info comes from logic part of our code into each creature`s manager script
            return base.CanDrag && manager.CanAttackNow;
        }
    }

    public override void OnStartDrag()
    {
        DebugManager.Instance.DebugMessage("Dragging with an attacking monster.", DebugManager.MessageType.Targeting, gameObject);
        whereIsThisCreature.VisualState = VisualStates.Dragging;
        // enable target graphic
        sr.enabled = true;
        // enable line renderer to start drawing the line.
        lr.enabled = true;
        // Show our potential targets!
        // Get our cardlogic
        CreatureLogic targeter = null;
        int uniqueID = gameObject.GetComponentInParent<IDHolder>().uniqueID;
        foreach (CreatureLogic cl in TurnManager.Instance.WhoseTurn.table.monstersOnTable) if (cl.ID == uniqueID) targeter = cl;
        // Highlight em!
        CardActionManager.Instance.HighlightValidTargets(TurnManager.Instance.WhoseTurn, targeter);
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            DebugManager.Instance.DebugMessage("We're dragging far from the card.", DebugManager.MessageType.Targeting, gameObject);
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f * direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            DebugManager.Instance.DebugMessage("We're dragging close to the card.", DebugManager.MessageType.Targeting, gameObject);
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }

    }

    public override void OnEndDrag()
    {
        if (DebugManager.Instance.showTargeting) Debug.Log("<color=purple>Dragging finished.</color>");
        Target = null;
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + this.transform.position).normalized, 
            maxDistance: 30f) ;

        foreach (RaycastHit h in hits)
        {
            if ((h.transform.tag == "TopPlayer" && this.tag == "BottomCreature") ||
                (h.transform.tag == "BottomPlayer" && this.tag == "TopCreature"))
            {
                if (DebugManager.Instance.showTargeting) Debug.Log("<color=purple>Target is opponent.</color>");
                // go face
                Target = h.transform.gameObject;
            }
            else if ((h.transform.tag == "TopCreature" && this.tag == "BottomCreature") ||
                    (h.transform.tag == "BottomCreature" && this.tag == "TopCreature"))
            {
                if (DebugManager.Instance.showTargeting) Debug.Log("<color=purple>Target is an enemy creature.</color>");
                // hit a creature, save parent transform
                Target = h.transform.parent.gameObject;
            }
            else
            {
                if (DebugManager.Instance.showTargeting) Debug.Log("<color=purple>The target couldn't be found.</color>");
            }
               
        }

        bool targetValid = false;

        if (Target != null)
        {
            int targetID = Target.GetComponent<IDHolder>().uniqueID;
            Debug.Log("Target ID: " + targetID);
            if (targetID == GlobalSettings.Instance.BottomPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
            {
                // attack character
                Debug.Log("Attacking "+Target);
                Debug.Log("TargetID: " + targetID);
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().uniqueID].GoFace();
                targetValid = true;
            }
            else if (CreatureLogic.CreaturesCreatedThisGame[targetID] != null)
            {
                // if targeted creature is still alive, attack creature
                targetValid = true;
                CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().uniqueID].AttackCreatureWithID(targetID);
                Debug.Log("Attacking "+Target);
            }
                
        }

        if (!targetValid)
        {
            // not a valid target, return
            if(tag.Contains("Bottom"))
                whereIsThisCreature.VisualState = VisualStates.BottomTable;
            else
                whereIsThisCreature.VisualState = VisualStates.TopTable;
            whereIsThisCreature.SetTableSortingOrder();
        }

        // return target and arrow to original position
        transform.localPosition = Vector3.zero;
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;

        // Dehighlight all

    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}
