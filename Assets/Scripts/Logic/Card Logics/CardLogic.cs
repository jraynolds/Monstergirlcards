using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CardLogic : IIdentifiable
{
    // reference to a player who owns this card
    Player owner;
    // a reference to the card asset that stores all the info about this card
    CardAsset ca;
    // effects on this card to modify its attributes, effects
    List<CardMod> Modifiers { get; set; }
    // effects bound to this card naturally
    List<Effect> Effects { get; set; }

    // PROPERTIES
    // an ID of this card
    private int uniqueCardID;
    public int ID
    {
        get { return uniqueCardID; }
        set
        {
            Debug.Log("hi!");
            uniqueCardID = value;
        }
    }

    public int CurrentManaCost { get; set; }

    public bool CanBePlayed
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.WhoseTurn == owner);
            // for spells the amount of characters on the field does not matter
            bool fieldNotFull = true;
            // but if this is a creature, we have to check if there is room on board (table)
            if (ca.MaxHealth > 0)
                fieldNotFull = (owner.table.monstersOnTable.Count < 7);
            //Debug.Log("Card: " + ca.name + " has params: ownersTurn=" + ownersTurn + "fieldNotFull=" + fieldNotFull + " hasMana=" + (CurrentManaCost <= owner.ManaLeft));
            return ownersTurn && fieldNotFull && (CurrentManaCost <= owner.ManaLeft);
        }
    }

    // CONSTRUCTOR
    public CardLogic(CardAsset ca)
    {
        // set the CardAsset reference
        this.ca = ca;
        // get unique int ID
        uniqueCardID = IDFactory.GetUniqueID();
        //UniqueCardID = IDFactory.GetUniqueID();
        ResetManaCost();
        // create an instance of SpellEffect with a name from our CardAsset
        // and attach it to 
        if (ca.SpellScriptName!= null && ca.SpellScriptName!= "")
        {
            effect = Activator.CreateInstance(Type.GetType(ca.SpellScriptName)) as SpellEffect;
        }
        // add this card to a dictionary with its ID as a key
        CardsCreatedThisGame.Add(ID, this);
    }

    // method to set or reset mana cost
    public void ResetManaCost()
    {
        CurrentManaCost = ca.ManaCost;
    }
}
