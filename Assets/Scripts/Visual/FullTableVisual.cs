using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class FullTableVisual : MonoBehaviour
{
    public Image table;
    public Image glow;
    public BoxCollider col;

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

    void Awake()
    {

    }
    
    void Update()
    {

    }
}
