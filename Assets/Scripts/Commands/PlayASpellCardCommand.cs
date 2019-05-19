using UnityEngine;
using System.Collections;

public class PlayASpellCardCommand: Command
{
    private ICardLogic card;
    private Player p;
    //private ICharacter target;

    public PlayASpellCardCommand(Player p, ICardLogic card)
    {
        this.card = card;
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        // move this card to the spot
        p.PArea.handVisual.PlayASpellFromHand(card.ID);
        // do all the visual stuff (for each spell separately????)
    }
}
