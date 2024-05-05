using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    PlayerStats playerStats;
    GameManager gameManager;
    EnemySpawner enemySpawner;
    CityManager cityManager;

    [Header("Wave Control")]
    [SerializeField] Button waveCallButton;
    [SerializeField] GameObject gameHud;

    [Header("Revive")]
    [SerializeField] Button reviveButton;
    public GameObject reviveUI;

    private void OnEnable()
    {
        cityManager = FindObjectOfType<CityManager>();

        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        waveCallButton.onClick.AddListener(OnWaveCallClicked);
        cityManager.OnWaveCalled += ConnectToSpawner;
    }

    private void OnDisable()
    {
        reviveButton.onClick.RemoveListener(OnReviveButtonPressed);
        waveCallButton.onClick.RemoveListener(OnWaveCallClicked);
        cityManager.OnWaveCalled -= ConnectToSpawner;
    }

    private void Start()
    {
        cityManager = FindObjectOfType<CityManager>();
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        waveCallButton.onClick.AddListener(OnWaveCallClicked);
        cityManager.OnWaveCalled += ConnectToSpawner;
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
    private void WaveCompleted()
    {
        waveCallButton.gameObject.SetActive(true);
    }
    private void ConnectToSpawner()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnWaveCompleted += WaveCompleted;
    }
}
