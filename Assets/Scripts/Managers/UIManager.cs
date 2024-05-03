using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    PlayerStats playerStats;
    GameManager gameManager;

    [Header("Wave Control")]
    [SerializeField] Button waveCallButton;
    [SerializeField] GameObject gameHud;

    [Header("Revive")]
    [SerializeField] Button reviveButton;
    public GameObject reviveUI;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        waveCallButton.onClick.AddListener(OnWaveCallClicked);
    }

    #region Revive

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

    #endregion

    private void OnWaveCallClicked()
    {
        gameManager.OnWaveCalled();
        waveCallButton.gameObject.SetActive(false);
    }
}
