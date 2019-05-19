using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class DragSpellNoTarget: DraggingActions{

    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        { 
            // TODO : include full field check
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        DebugManager.Instance.DebugMessage("Beginning to drag a spell.", DebugManager.MessageType.PlayerInteraction, gameObject);

        savedHandSlot = whereIsCard.Slot;

        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();

        int ID = transform.GetComponent<IDHolder>().uniqueID;
        Player p = TurnManager.Instance.WhoseTurn;
        ICardLogic cl = null;
        foreach (ICardLogic c in p.hand.cardsInHand) if (c.ID == ID) cl = c;
        CardActionManager.Instance.HighlightValidTargets(TurnManager.Instance.WhoseTurn, cl);

        DebugManager.Instance.DebugMessage("Highlighting entire table.", DebugManager.MessageType.Targeting);
        CardActionManager.Instance.HighlightValidTargets(p, cl);

    }

    public override void OnDraggingInUpdate()
    {
        
    }

    public override void OnEndDrag()
    {
        RaycastHit[] hits;
        // TODO: raycast here anyway, store the results in 
        hits = Physics.RaycastAll(origin: Camera.main.transform.position,
            direction: (-Camera.main.transform.position + this.transform.position).normalized,
            maxDistance: 30f);
        foreach (RaycastHit h in hits)
        {
            //Debug.Log(h, h.transform);
            if (h.transform.tag.Contains("Full Table") && h.transform.GetComponent<FullTableVisual>().IsValidTarget)
            {
                // hit the full table
                DebugManager.Instance.DebugMessage("You've selected the entire table.", DebugManager.MessageType.Targeting);
                PlayerOwner.PlayASpellFromHand(GetComponent<IDHolder>().uniqueID, -1);
                return;
            }
        }
        
        DebugManager.Instance.DebugMessage("Failed to find target.", DebugManager.MessageType.PlayerInteraction);
        // Set old sorting order
        whereIsCard.Slot = savedHandSlot;
        if (tag.Contains("Bottom"))
            whereIsCard.VisualState = VisualStates.BottomHand;
        else
            whereIsCard.VisualState = VisualStates.TopHand;
        // Move this card back to its slot position
        HandVisual PlayerHand = PlayerOwner.PArea.handVisual;
        Vector3 oldCardPos = PlayerHand.slots.Children[savedHandSlot].transform.localPosition;
        transform.DOLocalMove(oldCardPos, 1f);
    }

    protected override bool DragSuccessful()
    {
        return true;
    }
}
