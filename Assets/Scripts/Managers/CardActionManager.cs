using UnityEngine;
using System.Collections;
using System.Linq;

public class CardActionManager : MonoBehaviour
{
    public static CardActionManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // CARD TARGETING METHODS
    public bool CanThisCardTargetThatCard(CardLogic c1, CardLogic c2)
    {
        return true;
    }

    public bool CanThisCardTargetThatCard(CreatureLogic c1, CardLogic c2)
    {
        return true;
    }

    public bool CanThisCardTargetThatCard(CreatureLogic c1, CreatureLogic c2)
    {
        return true;
    }

    public bool CanThisCardTargetThatCard(CardLogic c1, CreatureLogic c2)
    {
        return true;
    }

    public bool CanThisCardTargetThatAvatar(CardLogic c1, Avatar a)
    {
        return true;
    }

    public bool CanThisCardTargetThatAvatar(CreatureLogic c1, Avatar a)
    {
        return true;
    }

    public bool DoesThisCardHaveAnyValidTargets(CardLogic c)
    {
        return true;
    }

    public bool DoesThisCardHaveAnyValidTargets(CreatureLogic c)
    {
        return true;
    }

    // OBJECT HIGHLIGHTING METHODS
    public void HighlightValidTargets(Player player, CardLogic targeter)
    {
        DebugManager.Instance.DebugMessage("Highlighting valid targets.", DebugManager.MessageType.Targeting);

        TargetingOptions[] targets = targeter.ca.targets;
        Player victim = TurnManager.Instance.WhoseTurn.OtherPlayer;
        bool isValid = true;

        if (targets.Contains(TargetingOptions.Enemy))
        {
            // check for spell blocking
            // check for spell block bypassing
            DebugManager.Instance.DebugMessage("Can target enemy: " + isValid, DebugManager.MessageType.Targeting);
            victim.PArea.avatarVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.EnemyHand))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in victim.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target enemy held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.EnemyHandCards))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in victim.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target enemy held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.EnemyCreatures))
        {
            // Check for hexproof
            // Check for hex bypassing
            foreach (CreatureLogic crl in victim.table.monstersOnTable)
            {
                GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
                if (g != null)
                {
                    DebugManager.Instance.DebugMessage("Can target enemy creature " + crl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCreatureManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.EnemyTable))
        {
            // check for hexproof
            // check for hex bypassing
            DebugManager.Instance.DebugMessage("Can target enemy table: " + isValid, DebugManager.MessageType.Targeting);
            victim.PArea.tableVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.You))
        {
            // check for spell blocking
            // check for spell block bypassing
            DebugManager.Instance.DebugMessage("Can target self: " + isValid, DebugManager.MessageType.Targeting);
            player.PArea.avatarVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.YourHand))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in player.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target player held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.YourHandCards))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in player.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target player held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.YourCreatures))
        {
            // Check for hexproof
            // Check for hex bypassing
            foreach (CreatureLogic crl in player.table.monstersOnTable)
            {
                GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
                if (g != null)
                {
                    DebugManager.Instance.DebugMessage("Can target own creature " + crl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCreatureManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.YourTable))
        {
            // check for hexproof
            // check for hex bypassing
            DebugManager.Instance.DebugMessage("Can target own table: " + isValid, DebugManager.MessageType.Targeting);
            player.PArea.tableVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.NoTarget))
        {
            // check for hexproof
            // check for hex bypassing
            DebugManager.Instance.DebugMessage("Can play targetless: " + isValid, DebugManager.MessageType.Targeting);
            GlobalSettings.Instance.HighlightTable(isValid);
        }
    }

    public void HighlightValidTargets(Player player, CreatureLogic targeter)
    {
        DebugManager.Instance.DebugMessage("Highlighting valid targets.", DebugManager.MessageType.Targeting);

        TargetingOptions[] targets = targeter.ca.targets;
        Player victim = TurnManager.Instance.WhoseTurn.OtherPlayer;
        bool isValid = true;

        if (targets.Contains(TargetingOptions.Enemy))
        {
            // check for taunt minions
            // check for taunt bypassing
            // check for ability to attack the enemy
            DebugManager.Instance.DebugMessage("Can target enemy: " + isValid, DebugManager.MessageType.Targeting);
            victim.PArea.avatarVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.EnemyHand))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in victim.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target enemy held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.EnemyHandCards))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in victim.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target enemy held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.EnemyCreatures))
        {
            // Check for taunt minions
            // Check for taunt bypassing
            // check for ability to attack creatures
            foreach (CreatureLogic crl in victim.table.monstersOnTable)
            {
                GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
                if (g != null)
                {
                    DebugManager.Instance.DebugMessage("Can target enemy creature " + crl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCreatureManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.EnemyTable))
        {
            // check for hexproof
            // check for hex bypassing
            DebugManager.Instance.DebugMessage("Can target enemy table: " + isValid, DebugManager.MessageType.Targeting);
            victim.PArea.tableVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.You))
        {
            // check for spell blocking
            // check for spell block bypassing
            DebugManager.Instance.DebugMessage("Can target self: " + isValid, DebugManager.MessageType.Targeting);
            player.PArea.avatarVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.YourHand))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in player.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target player held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.YourHandCards))
        {
            // check for hexproof
            // check for hex bypassing
            foreach (CardLogic cl in player.hand.cardsInHand)
            {
                GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
                if (g != null)
                {
                    // check for hexproof
                    // check for hex bypassing
                    DebugManager.Instance.DebugMessage("Can target player held card " + cl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCardManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.YourCreatures))
        {
            // Check for hexproof
            // Check for hex bypassing
            foreach (CreatureLogic crl in player.table.monstersOnTable)
            {
                GameObject g = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
                if (g != null)
                {
                    DebugManager.Instance.DebugMessage("Can target own creature " + crl.ca.name + ": " + isValid, DebugManager.MessageType.Targeting);
                    g.GetComponent<OneCreatureManager>().IsValidTarget = isValid;
                }
            }
        }
        if (targets.Contains(TargetingOptions.YourTable))
        {
            // check for hexproof
            // check for hex bypassing
            DebugManager.Instance.DebugMessage("Can target own table: " + isValid, DebugManager.MessageType.Targeting);
            player.PArea.tableVisual.IsValidTarget = isValid;
        }
        if (targets.Contains(TargetingOptions.NoTarget))
        {
            // check for hexproof
            // check for hex bypassing
            DebugManager.Instance.DebugMessage("Can play targetless: " + isValid, DebugManager.MessageType.Targeting);
            GlobalSettings.Instance.HighlightTable(isValid);
        }
    }

    public void UnHighlightAll(bool highlight = false)
    {
        DebugManager.Instance.DebugMessage("Unhighlighting everything? " + !highlight, DebugManager.MessageType.Targeting);

        GlobalSettings.Instance.HighlightTable(highlight);

        foreach (Player p in GlobalSettings.Instance.Players.Values)
        {
            p.PArea.avatarVisual.IsValidTarget = highlight;
            p.PArea.handVisual.HighlightCards(highlight);
            p.PArea.tableVisual.IsValidTarget = highlight;
            p.PArea.tableVisual.HighlightCards(highlight);
        }
    }

    public void HighlightCastableCards(Player p)
    {
        DebugManager.Instance.DebugMessage("Highlighting castables for player " + p.name, DebugManager.MessageType.Targeting);

        foreach (CardLogic cl in p.hand.cardsInHand)
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
            if (g != null)
            {
                bool canCast = (g.GetComponent<WhereIsTheCardOrCreature>().VisualState != VisualStates.Transition) && DoesThisCardHaveAnyValidTargets(cl);
                g.GetComponent<OneCardManager>().CanBePlayedNow = canCast;
            }
        }
    }

    public void HighlightMonstersWithAttacks(Player p)
    {
        DebugManager.Instance.DebugMessage("Highlighting attackerbles for player " + p.name, DebugManager.MessageType.Targeting);

        foreach (CreatureLogic cl in p.table.monstersOnTable)
        {
            GameObject g = IDHolder.GetGameObjectWithID(cl.ID);
            if (g != null)
            {
                g.GetComponent<OneCreatureManager>().CanAttackNow = cl.CanAttack;
            }
        }
    }
}
