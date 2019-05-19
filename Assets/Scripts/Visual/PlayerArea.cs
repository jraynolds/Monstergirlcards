using UnityEngine;
using System.Collections;

public enum AreaPosition{Top, Bottom}

public class PlayerArea : MonoBehaviour 
{
    public AreaPosition owner;
    // allows or blocks controls for the player.
    public bool ControlsON = true;
    public PlayerDeckVisual PDeck;
    public ManaPoolVisual manaPoolVisual;
    public HandVisual handVisual;
    public TableVisual tableVisual;
    public AvatarVisual avatarVisual;

    public bool AllowedToControlThisPlayer
    {
        get;
        set;
    }      


}
