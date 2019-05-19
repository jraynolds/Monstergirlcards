using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {

    public static DebugManager Instance { get; set; }

    public enum MessageType { System, Game, Creation, Loading, Animation, Commands, Targeting, PlayerInteraction, Error }

    [Header("Select how much info the debugger will show.")]
    public bool showSystem = true;
    public bool showGame = true;
    public bool showCreation = false;
    public bool showLoading = false;
    public bool showAnimation = false;
    public bool showCommands = false;
    public bool showTargeting = false;
    public bool showPlayerInteractions = false;
    public bool showErrors = true;

    [Header("Select colors for each message.")]
    public Color systemColor = Color.gray;
    public Color gameColor = new Color(0.3f, 0.4f, 1.0f);
    public Color creationColor = Color.blue;
    public Color loadingColor = new Color(0.8f, 0.3f, 0.3f);
    public Color animationColor = Color.green;
    public Color commandsColor = Color.magenta;
    public Color targetingColor = Color.yellow;
    public Color playerInteractionColor = new Color(1.0f, 0.4f, 0.1f);
    public Color errorColor = Color.red;

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

    // Why doesn't Unity support dicts
    public void DebugMessage(string contents, MessageType type, GameObject target = null)
    {
        if (type == MessageType.System)
        {
            if (showSystem) SendDebugMessage(contents, systemColor, target);
        }
        else if (type == MessageType.Game)
        {
            if (showGame) SendDebugMessage(contents, gameColor, target);
        }
        else if (type == MessageType.Creation)
        {
            if (showCreation) SendDebugMessage(contents, creationColor, target);
        }
        else if (type == MessageType.Loading)
        {
            if (showLoading) SendDebugMessage(contents, loadingColor, target);
        }
        else if (type == MessageType.Animation)
        {
            if (showAnimation) SendDebugMessage(contents, animationColor, target);
        }
        else if (type == MessageType.Commands)
        {
            if (showCommands) SendDebugMessage(contents, commandsColor, target);
        }
        else if (type == MessageType.Targeting)
        {
            if (showTargeting) SendDebugMessage(contents, targetingColor, target);
        }
        else if (type == MessageType.PlayerInteraction)
        {
            if (showPlayerInteractions) SendDebugMessage(contents, playerInteractionColor, target);
        }
        else if (type == MessageType.Error)
        {
            if (showErrors) SendDebugMessage(contents, errorColor, target);
        }
    }

    private void SendDebugMessage(string contents, Color color, GameObject target = null)
    {
        string colorString = string.Format("#{0:X2}{1:X2}{2:X2}", (byte)(color.r * 255f), (byte)(color.g * 255f), (byte)(color.b * 255f));
        string output = string.Format("<color={0}>{1}</color>", colorString, contents);
        if (target != null) Debug.Log(output, target);
        else Debug.Log(output);
    }

}
