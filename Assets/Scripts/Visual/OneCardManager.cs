using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")]
    public Text NameText;
    public Text ManaCostText;
    public Text DescriptionText;
    public Text HealthText;
    public Text AttackText;
    [Header("Image References")]
    public Image graphic;
    public Image frontGlow;
    public Image backGlow;

    void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset();
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            frontGlow.enabled  = value;

            DebugManager.Instance.DebugMessage(string.Format("{0} can be played? {1}.", cardAsset.name, canBePlayedNow), DebugManager.MessageType.Targeting, gameObject);
        }
    }

    private bool isValidTarget = false;
    public bool IsValidTarget
    {
        get
        {
            return isValidTarget;
        }

        set
        {
            isValidTarget = value;

            frontGlow.enabled  = value;

            DebugManager.Instance.DebugMessage(string.Format("{0} can be targeted? {1}.", cardAsset.name, isValidTarget), DebugManager.MessageType.Targeting, gameObject);
        }
    }

    public void ReadCardFromAsset()
    {
        if (PreviewManager != null)
        {
            string cardType = "spell";
            if (cardAsset.MaxHealth != 0) cardType = "creature";
            string output = string.Format("Reading a new card: {0}. It is a {1}.", cardAsset.name, cardType);
            DebugManager.Instance.DebugMessage(output, DebugManager.MessageType.Loading);
        }
        // 2) add card name
        NameText.text = cardAsset.name;
        // 3) add mana cost
        ManaCostText.text = cardAsset.ManaCost.ToString();
        // 4) add description
        DescriptionText.text = cardAsset.Description;
        // 5) Change the card graphic sprite
        graphic.sprite = cardAsset.CardImage;

        if (cardAsset.MaxHealth != 0)
        {
            // this is a creature
            AttackText.text = cardAsset.Attack.ToString();
            HealthText.text = cardAsset.MaxHealth.ToString();
        }

        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }

        if (PreviewManager != null) DebugManager.Instance.DebugMessage("Read finished.", DebugManager.MessageType.Loading);
    }
}
