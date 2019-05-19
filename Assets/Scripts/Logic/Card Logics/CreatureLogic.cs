using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic : ICharacter 
{
    // PUBLIC FIELDS
    public Player owner;
    public CardAsset ca;
    
    public List<Effect> effects = new List<Effect>();

    // PROPERTIES
    // property from ICharacter interface
    public int ID
    {
        get; set;
    }

    // health
    public int BaseHealth { get; set; }

    private int _health;
    public int Health
    {
        get { return _health; }

        set
        {
            if (value <= 0)
                Die();
            else
                _health = value;
        }
    }

    // attack
    public int Attack { get; set; }


    // number of attacks for one turn if (attacksForOneTurn==2) => Windfury
    public int attacksForOneTurn = 1;
    public int AttacksLeftThisTurn
    {
        get;
        set;
    }


    // CONSTRUCTOR
    public CreatureLogic(Player owner, CardAsset ca)
    {
        this.ca = ca;
        baseHealth = Health = ca.MaxHealth;
        baseAttack = Attack = ca.Attack;
        attacksForOneTurn = ca.AttacksForOneTurn;
        // AttacksLeftThisTurn is now equal to 0
        if (ca.Charge)
            AttacksLeftThisTurn = attacksForOneTurn;
        this.owner = owner;
        UniqueCreatureID = IDFactory.GetUniqueID();
        if (ca.CreatureScriptName!= null && ca.CreatureScriptName!= "")
        {
            effect = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[]{owner, this, ca.specialCreatureAmount}) as CreatureEffect;
            effect.RegisterEventEffect();
        }
        CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
    }

    // METHODS
    public void OnTurnStart()
    {
        AttacksLeftThisTurn = attacksForOneTurn;
    }
    
    // returns true if we can attack with this creature now
    public bool CanAttack
    {
        get
        {
            bool ownersTurn = (TurnManager.Instance.WhoseTurn == owner);
            return (ownersTurn && (AttacksLeftThisTurn > 0) && !Frozen && CardActionManager.Instance.DoesThisCardHaveAnyValidTargets(this));
        }
    }

    public void Die()
    {
        DebugManager.Instance.DebugMessage(ca.name + " has died!", DebugManager.MessageType.Game);

        owner.table.monstersOnTable.Remove(this);

        // cause Deathrattle Effect
        if (effect != null)
        {
            effect.WhenACreatureDies();
            effect.UnRegisterEventEffect();
            effect = null;
        }

        new CreatureDieCommand(UniqueCreatureID, owner).AddToQueue();
    }

    public void GoFace()
    {
        AttacksLeftThisTurn--;
        int targetHealthAfter = owner.OtherPlayer.Health - Attack;
        new CreatureAttackCommand(owner.OtherPlayer.PlayerID, UniqueCreatureID, 0, Attack, Health, targetHealthAfter).AddToQueue();
        owner.OtherPlayer.Health -= Attack;
    }

    public void AttackCreature (CreatureLogic target)
    {
        AttacksLeftThisTurn--;
        // calculate the values so that the creature does not fire the DIE command before the Attack command is sent
        int targetHealthAfter = target.Health - Attack;
        int attackerHealthAfter = Health - target.Attack;
        new CreatureAttackCommand(target.UniqueCreatureID, UniqueCreatureID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter).AddToQueue();

        target.Health -= Attack;
        Health -= target.Attack;
    }

    public void AttackCreatureWithID(int uniqueCreatureID)
    {
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target);
    }

    // STATIC For managing IDs
    public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();

}
