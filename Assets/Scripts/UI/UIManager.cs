using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{

    private PlayerStats playerStats;
    private GameManager gameManager;
    private EarningsHolder earningsHolder;

    [Header("Earnings")]
    [SerializeField] private GameObject poolObject;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject meatPrefab;

    public Queue<GameObject> coinPool = new Queue<GameObject>();
    public Queue<GameObject> meatPool = new Queue<GameObject>();
    // private Queue<GameObject> expQueue = new Queue<GameObject>();


    [Header("Wave Control")]
    [SerializeField] private Button waveCallButton;

    [Header("Panels")]
    [SerializeField] private List<PanelBase> allPanels;
    [SerializeField] private GameObject joystickPanel;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private PlayerDeathPanel playerDeathPanel;
    [SerializeField] private UpgradePanel upgradePanel;
    [SerializeField] private WaveWonPanel waveWonPanel;
    [SerializeField] private TowerDeathPanel towerDeathPanel;

    [Header("Upgrading")]
    [SerializeField] private GameObject scrollPanel;
    [SerializeField] private Button enableUpgradeHudButton;
    [SerializeField] private Button exitUpgradeHudButton;

    #region  On Enable - On Disable

    private void OnEnable()
    {

        playerStats = PlayerStats.instance;
        earningsHolder = EarningsHolder.instance;
        gameManager = GameManager.instance;

        playerStats.OnWaveWon += WaveCompleted;


        // Geçici olarak böyle yapildi (MVP'ye reklam entegrasyonu yetişirse)
        // onwavewondan kaldirip button aksiyonuna taşınacak ve wavewon ekranında
        // wavewon paneli açılacak
        // şuanda wave kazanılınca direkt olarak playera atıyor


        waveCallButton.onClick.AddListener(OnWaveCallClicked);

        // Death & Revive
        playerStats.OnPlayerRevived += DeathReviveSequenceEnd;
        playerStats.OnPlayerKilled += DeathReviveSequence;

        playerStats.OnWaveWon += EnableWaveWonUI;



        // Lose Game
        for (int i = 0; i < gameManager.allCities.Count; i++)
        {
            gameManager.allCities[i].GetTower().OnTargetDestroyed += GameLostPanelSequence;
        }


        // Upgrade Button

        enableUpgradeHudButton.onClick.AddListener(EnableUpgradeHud);
        exitUpgradeHudButton.onClick.AddListener(DisableUpgradeHud);

    }


    private void OnDisable()
    {
        // Revive & Wave Control Buttons
        waveCallButton.onClick.RemoveAllListeners();


        playerStats.OnWaveWon -= EnableWaveWonUI;
        playerStats.OnWaveWon -= WaveCompleted;

        // Death & Revive
        playerStats.OnPlayerRevived -= DeathReviveSequenceEnd;
        playerStats.OnPlayerKilled -= DeathReviveSequence;


        // Lose Game
        for (int i = 0; i < gameManager.allCities.Count; i++)
        {
            gameManager.allCities[i].GetTower().OnTargetDestroyed -= GameLostPanelSequence;
        }

        // Upgrading Button Cleanup
        enableUpgradeHudButton.onClick.RemoveAllListeners();
        exitUpgradeHudButton.onClick.RemoveAllListeners();

    }

    #endregion

    #region  Start
    private void Start()
    {
        DisableUpgradingButton();
        SetStartingUI();
        EarningPooler();
    }

    private void SetStartingUI()
    {
        GetBackToGamePanel();
    }

    #endregion

    #region  Wave Related
    private void OnWaveCallClicked()
    {
        if (gameManager.isAttackPhase) return;

        gameManager.OnWaveCalled();
        DisableUpgradingButton();
        DisableUpgradeHud();

        gamePanel.OnWaveCallClicked();

        waveCallButton.gameObject.SetActive(false);
    }

    public void UpdateInWaveProgressBarValue(float value)
    {
        gamePanel.UpdateInWaveProgressBarValue(value);
    }


    private void WaveCompleted()
    {
        waveCallButton.gameObject.SetActive(true);
        gamePanel.UpdateAllWavesProgressBar();
    }

    #endregion

    #region Earnings
    public void OnApplyEarningToPlayerButtonClicked(float newMultipiler)
    {
        earningsHolder.OnBonusMultiplierApplied?.Invoke(newMultipiler);
    }
    #endregion

    public void EnableWaveWonUI()
    {
        ActivatePanel(waveWonPanel.gameObject);

        waveWonPanel.EnableWaveWonUI();
    }

    public void GameLostPanelSequence()
    {
        DeactivateAllPanels();
        ActivatePanel(towerDeathPanel.gameObject);
        towerDeathPanel.GameLostPanelSequence();
    }

    public void EarningPooler()
    {
        // pool coins
        for (int i = 0; i < 10; i++)
        {
            GameObject coin = Instantiate(coinPrefab, Vector3.zero, Quaternion.identity, poolObject.transform);
            coin.transform.localPosition = Vector3.zero;

            coin.SetActive(false);
            coinPool.Enqueue(coin);
        }
        // pool meat
        for (int i = 0; i < 10; i++)
        {
            GameObject meat = Instantiate(meatPrefab, Vector3.zero, Quaternion.identity, poolObject.transform);
            meat.transform.localPosition = Vector3.zero;

            meat.SetActive(false);
            meatPool.Enqueue(meat);
        }
    }
    #region Panel Management

    public void DeactivateAllPanels()
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].gameObject.SetActive(false);
        }
    }

    public void ActivatePanel(GameObject panelToActive, GameObject panelToActive2 = null)
    {
        if (panelToActive != null)
        {
            panelToActive.SetActive(true);
        }
        if (panelToActive2 != null)
        {
            panelToActive2.SetActive(true);
        }
    }

    public void DeactivatePanel(GameObject panelToDeactive)
    {
        if (panelToDeactive != null)
        {
            panelToDeactive.SetActive(false);
        }
    }
    public void GetBackToGamePanel()
    {
        DeactivateAllPanels();

        ActivatePanel(gamePanel.gameObject, joystickPanel);
    }


    #endregion

    #region Upgrade Panel Management

    private void EnableUpgradeHud()
    {
        ActivatePanel(upgradePanel.gameObject);
    }

    private void DisableUpgradeHud()
    {
        DeactivatePanel(upgradePanel.gameObject);
        scrollPanel.transform.localPosition = Vector3.zero;
    }

    #endregion

    #region  Player Death Panel Management
    private void DeathReviveSequence()
    {
        DeactivateAllPanels();
        ActivatePanel(playerDeathPanel.gameObject);

        playerDeathPanel.DeathReviveSequence();
    }

    private void DeathReviveSequenceEnd()
    {
        GetBackToGamePanel();
    }
    #endregion

    #region  Update Texts - Text Related

    public void UpdateText(TMP_Text textToUpdate, int value, string newString = null)
    {
        if (textToUpdate == null)
        {
            Debug.LogWarning("textToUpdate is null");
            return;
        }

        if (newString != null)
        {
            textToUpdate.text = newString + " " + value.ToString();
        }
        else
        {
            textToUpdate.text = value.ToString();
        }

    }



    #endregion

    #region Upgrading Button

    public void EnableUpgradingButton()
    {
        enableUpgradeHudButton.gameObject.SetActive(true);
    }
    public void DisableUpgradingButton()
    {
        enableUpgradeHudButton.gameObject.SetActive(false);
    }

    #endregion


    #region Getters
    public GamePanel GetGamePanel()
    {
        return gamePanel;
    }
    #endregion
}