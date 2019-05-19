using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragSpellOnTarget : DraggingActions {

    public TargetingOptions[] targets;
    private SpriteRenderer sr;
    private LineRenderer lr;
    private WhereIsTheCardOrCreature whereIsThisCard;
    private VisualStates tempVisualState;
    private Transform triangle;
    private SpriteRenderer triangleSR;
    private GameObject target;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        { 
            // TEST LINE: this is just to test playing creatures before the game is complete 
            // return true;

            // TODO : include full field check
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponentInChildren<LineRenderer>();
        //lr.sortingLayerName = "Above Everything";
        triangle = transform.Find("Triangle");
        triangleSR = triangle.GetComponent<SpriteRenderer>();
        
        manager = GetComponentInParent<OneCardManager>();
        whereIsThisCard = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override void OnStartDrag()
    {
        tempVisualState = whereIsThisCard.VisualState;
        whereIsThisCard.VisualState = VisualStates.Dragging;
        sr.enabled = true;
        lr.enabled = true;

        int ID = transform.parent.GetComponent<IDHolder>().uniqueID;
        Player p = TurnManager.Instance.WhoseTurn;
        ICardLogic cl = null;
        foreach (ICardLogic c in p.hand.cardsInHand) if (c.ID == ID) cl = c;
        CardActionManager.Instance.HighlightValidTargets(TurnManager.Instance.WhoseTurn, cl);
    }

    public override void OnDraggingInUpdate()
    {
        // This code only draws the arrow
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction*2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            lr.SetPositions(new Vector3[]{ transform.parent.position, transform.position - direction*2.3f });
            lr.enabled = true;

            // position the end of the arrow between near the target.
            triangleSR.enabled = true;
            triangleSR.transform.position = transform.position - 1.5f*direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            lr.enabled = false;
            triangleSR.enabled = false;
        }

    }

    public override void OnEndDrag()
    {
        target = null;
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);
        foreach (RaycastHit h in hits)
        {
            //Debug.Log(h, h.transform);
            // We're only looking for valid targets
            if (h.transform.tag.Contains("Player") && h.transform.GetComponent<AvatarVisual>().IsValidTarget)
            {
                // selected a Player
                DebugManager.Instance.DebugMessage("You've selected a player.", DebugManager.MessageType.Targeting);
                target = h.transform.gameObject;
                break;
            }
            else if (h.transform.tag.Contains("Creature") && h.transform.parent.GetComponent<OneCreatureManager>().IsValidTarget)
            {
                // hit a creature, save parent transform
                DebugManager.Instance.DebugMessage("You've selected a creature.", DebugManager.MessageType.Targeting);
                target = h.transform.parent.gameObject;
                break;
            }
            else if (h.transform.tag.Contains("HandCard"))
            {
                if ((h.transform.GetComponent<OneCardManager>() && h.transform.GetComponent<OneCardManager>().IsValidTarget) 
                    || (h.transform.parent.GetComponent<OneCardManager>() && h.transform.parent.GetComponent<OneCardManager>().IsValidTarget))
                {
                    // hit a spell, we're not ready yet
                    DebugManager.Instance.DebugMessage("You've selected a card.", DebugManager.MessageType.Targeting);
                    throw new NotImplementedException();
                }
            }
            else if (h.transform.tag.Contains("Full Table") && h.transform.GetComponent<FullTableVisual>().IsValidTarget)
            {
                // hit the full table, we're not ready yet
                DebugManager.Instance.DebugMessage("You've selected the entire table.", DebugManager.MessageType.Targeting);
                throw new NotImplementedException();
            }
        }

        if (target != null)
        {
            // determine an owner of this card
            Player owner = null; 
            if (tag.Contains("Bottom"))
                owner = GlobalSettings.Instance.BottomPlayer;
            else
                owner = GlobalSettings.Instance.TopPlayer;
            
            int targetID = target.GetComponent<IDHolder>().uniqueID;

            owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().uniqueID, targetID);
        }
        else
        {
            // not a valid target, return
            whereIsThisCard.VisualState = tempVisualState;
            whereIsThisCard.SetHandSortingOrder();
        }

        // return target and arrow to original position
        // this position is special for spell cards to show the arrow on top
        transform.localPosition = new Vector3(0f, 0f, 0.4f);
        sr.enabled = false;
        lr.enabled = false;
        triangleSR.enabled = false;
    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}
