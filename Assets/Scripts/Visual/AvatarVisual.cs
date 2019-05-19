using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class AvatarVisual : MonoBehaviour
{
    public AreaPosition owner;
    public CharacterAsset charAsset;
    public Image charImage;
    public Text HealthText;
    public Image glow;

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

            if (glow != null) glow.enabled = value;
        }
    }

    void Awake()
    {
        if (charAsset != null)
            ApplyLookFromAsset();
    }

    public void ApplyLookFromAsset()
    {
        HealthText.text = charAsset.MaxHealth.ToString();
        if (charImage) charImage.sprite = charAsset.AvatarImage;
    }

    public void TakeDamage(int amount, int healthAfter)
    {
        DamageEffect.CreateDamageEffect(transform.position, amount);
        HealthText.text = healthAfter.ToString();
        DebugManager.Instance.DebugMessage(string.Format("{0} player takes {1} damage, leaving them at {2}!", owner, amount, healthAfter), DebugManager.MessageType.Game);
    }

    public void Explode()
    {
        Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
        Sequence s = DOTween.Sequence();
        s.PrependInterval(2f);
        s.OnComplete(() => GlobalSettings.Instance.GameOverPanel.SetActive(true));
    }

}
