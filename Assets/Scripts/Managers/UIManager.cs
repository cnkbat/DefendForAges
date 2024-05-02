using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    PlayerStats playerStats;

    [Header("UI Elements")]
    public GameObject reviveUI;
    [SerializeField] Button reviveButton;


    private void Start()
    {
        playerStats = PlayerStats.instance;
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
    }

    public void HandleReviveUI()
    {
        if (reviveUI.activeSelf)
        {
            reviveUI.SetActive(false);
        }
        else
        {
            reviveUI.SetActive(true);
        }
    }

    private void OnReviveButtonPressed()
    {
        playerStats.OnRevive?.Invoke();
    }
}
