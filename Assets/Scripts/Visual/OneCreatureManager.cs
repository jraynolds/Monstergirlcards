using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneCreatureManager : MonoBehaviour 
{
    public CardAsset cardAsset;
    public OneCardManager previewManager;
    [Header("Text Component References")]
    public Text healthText;
    public Text attackText;
    [Header("Image References")]
    public Image graphic;
    public Image glow;

    void Awake()
    {
        if (cardAsset != null)
            ReadCreatureFromAsset();
    }

    private bool canAttackNow = false;
    public bool CanAttackNow
    {
        get
        {
            return canAttackNow;
        }

        set
        {
            canAttackNow = value;

            glow.enabled = value;
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

            glow.enabled = value;
        }
    }

    public void ReadCreatureFromAsset()
    {
        // Change the card graphic sprite
        graphic.sprite = cardAsset.CardImage;

        attackText.text = cardAsset.Attack.ToString();
        healthText.text = cardAsset.MaxHealth.ToString();

        if (previewManager != null)
        {
            previewManager.cardAsset = cardAsset;
            previewManager.ReadCardFromAsset();
        }
    }	

    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount != 0)
        {
            DamageEffect.CreateDamageEffect(transform.position, amount);
            healthText.text = previewManager.HealthText.text = healthAfter.ToString();
        }
        DebugManager.Instance.DebugMessage(string.Format("{0} takes {1} damage, leaving it at {2}!", cardAsset.name, amount, healthAfter), DebugManager.MessageType.Game);
    }

    public void ChangeStat(string stat, int value)
    {
        if (stat == "health")
        {
            // do visuals
            healthText.text = previewManager.HealthText.text = value.ToString();
            DebugManager.Instance.DebugMessage(string.Format("{0}'s health has changed to {1}!", cardAsset.name, value), DebugManager.MessageType.Game);
            // 0 check should come before this
        }
        else if (stat == "attack")
        {
            // do visuals
            attackText.text = previewManager.AttackText.text = value.ToString();
            DebugManager.Instance.DebugMessage(string.Format("{0}'s attack has changed to {1}!", cardAsset.name, value), DebugManager.MessageType.Game);
        }
    }
}
