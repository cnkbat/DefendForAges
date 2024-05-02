using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [Header("UI Elements")]
    public GameObject reviveUI;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void HandleReviveUI()
    {
        if(reviveUI.activeSelf)
        {
            reviveUI.SetActive(false);
        }
        else
        {
            reviveUI.SetActive(true);
        }
    }
}
