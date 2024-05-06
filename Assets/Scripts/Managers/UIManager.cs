using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] RPGSystemSO playerSO;
    PlayerStats playerStats;
    GameManager gameManager;
    EnemySpawner enemySpawner;
    CityManager cityManager;

    [Header("GameHud Texts")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text waveIndexText;

    [Header("Wave Control")]
    [SerializeField] private Button waveCallButton;
    [SerializeField] private GameObject gameHud;

    [Header("Revive")]
    [SerializeField] private Button reviveButton;
    [SerializeField] private GameObject reviveUI;

    [Header("---- Upgrading ----")]

    [Header("Upgrade Buttons")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button upgradeHudButton;
    [SerializeField] private Button exitUpgradeHudButton;
    [SerializeField] private Button upgradeAttackSpeedButton;
    [SerializeField] private Button upgradeDamageButton;
    [SerializeField] private Button upgradeLifeStealButton;
    [SerializeField] private Button upgradeMovementSpeedButton;
    [SerializeField] private Button upgradePowerupDurButton;
    [SerializeField] private Button upgradeMaxHealthButton;
    [SerializeField] private Button upgradeDualWeaponButton;

    [Header("!--Upgrade Texts--!")]

    [Header("Attack Speed Texts")]
    [SerializeField] private TMP_Text attackSpeedCurrentText;
    [SerializeField] private TMP_Text attackSpeedNextText;
    [SerializeField] private TMP_Text attackSpeedCostText;

    [Header("Damage Texts")]
    [SerializeField] private TMP_Text damageCurrentText;
    [SerializeField] private TMP_Text damageNextText;
    [SerializeField] private TMP_Text damageCostText;

    [Header("Life Steal Texts")]

    [SerializeField] private TMP_Text lifeStealCurrentText;
    [SerializeField] private TMP_Text lifeStealNextText;
    [SerializeField] private TMP_Text lifeStealCostText;


    [Header("Movement Speed Texts")]
    [SerializeField] private TMP_Text movementSpeedCurrentText;
    [SerializeField] private TMP_Text movementSpeedNextText;
    [SerializeField] private TMP_Text movementSpeedCostText;

    [Header("Powerup Dur Texts")]
    [SerializeField] private TMP_Text powerupDurCurrentText;
    [SerializeField] private TMP_Text powerupDurNextText;
    [SerializeField] private TMP_Text powerupDurCostText;

    [Header("Max Health Texts")]
    [SerializeField] private TMP_Text maxHealthCurrentText;
    [SerializeField] private TMP_Text maxHealthNextText;
    [SerializeField] private TMP_Text maxHealthCostText;

    [Header("Dual Weapon Texts")]
    [SerializeField] private TMP_Text dualWeaponCostText;

    [Header("!--- Upgrade Text Visuals ---!")]
    [SerializeField] private List<float> attackSpeedVisualValues;
    [SerializeField] private List<float> damageVisualValues;
    [SerializeField] private List<float> lifeStealVisualValues;
    [SerializeField] private List<float> movementSpeedVisualValues;

    #region  On Enable - On Disable

    private void OnEnable()
    {

        playerStats = PlayerStats.instance;

        cityManager = FindObjectOfType<CityManager>();

        // Revive & Wave Control
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        waveCallButton.onClick.AddListener(OnWaveCallClicked);
        cityManager.OnWaveCalled += ConnectToSpawner;

        // Text Updates
        playerStats.OnXPChange += UpdateXPText;
        playerStats.OnMoneyChange += UpdateMoneyText;


        // Upgrading
        playerStats.OnUpgradeCompleted += UpdateUpgradeUI;

        upgradeHudButton.onClick.AddListener(EnableDisableUpgradeHud);
        exitUpgradeHudButton.onClick.AddListener(EnableDisableUpgradeHud);
        upgradeAttackSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeAttackSpeed);
        upgradeDamageButton.onClick.AddListener(playerStats.AttemptUpgradeDamage);
        upgradeLifeStealButton.onClick.AddListener(playerStats.AttemptUpgradeLifeSteal);
        upgradeMovementSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeMovementSpeed);
        upgradePowerupDurButton.onClick.AddListener(playerStats.AttemptUpgradePowerupDuration);
        upgradeMaxHealthButton.onClick.AddListener(playerStats.AttemptUpgradeMaxHealth);
        upgradeDualWeaponButton.onClick.AddListener(playerStats.AttemptUpgradeDualWeapon);

    }

    private void OnDisable()
    {
        // Revive & Wave Control
        reviveButton.onClick.RemoveAllListeners();
        waveCallButton.onClick.RemoveAllListeners();
        cityManager.OnWaveCalled -= ConnectToSpawner;


        // Text Updates
        playerStats.OnXPChange -= UpdateXPText;
        playerStats.OnMoneyChange -= UpdateMoneyText;


        // Upgrading
        playerStats.OnUpgradeCompleted -= UpdateUpgradeUI;

        upgradeHudButton.onClick.RemoveAllListeners();
        exitUpgradeHudButton.onClick.RemoveAllListeners();
        upgradeAttackSpeedButton.onClick.RemoveAllListeners();
        upgradeDamageButton.onClick.RemoveAllListeners();
        upgradeLifeStealButton.onClick.RemoveAllListeners();
        upgradeMovementSpeedButton.onClick.RemoveAllListeners();
        upgradePowerupDurButton.onClick.RemoveAllListeners();
        upgradeMaxHealthButton.onClick.RemoveAllListeners();


    }

    #endregion

    #region  Start
    private void Start()
    {
        cityManager = FindObjectOfType<CityManager>();
        cityManager.OnWaveCalled += ConnectToSpawner;
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        SetStartingUI();
    }
    private void SetStartingUI()
    {
        UpdateText(waveIndexText, playerStats.waveIndex, "Wave");
        UpdateText(moneyText, playerStats.money, "Money");
        UpdateText(xpText, playerStats.experiencePoint, "XP");

        playerSO = playerStats.GetPlayerSO();
        UpdateUpgradeUI();
    }

    #endregion

    #region Upgrade
    private void EnableDisableUpgradeHud()
    {
        upgradePanel.SetActive(!upgradePanel.activeSelf);
    }

    private void UpdateUpgradeUI()
    {

        // ATTACK SPEED
        UpdateUpgradeText(attackSpeedCurrentText, attackSpeedVisualValues[playerStats.attackSpeedIndex]);
        UpdateUpgradeText(attackSpeedNextText, attackSpeedVisualValues[playerStats.attackSpeedIndex + 1]);
        UpdateUpgradeText(attackSpeedCostText, playerSO.GetAttackSpeedCosts()[playerStats.attackSpeedIndex]);

        // DAMAGE
        UpdateUpgradeText(damageCurrentText, damageVisualValues[playerStats.damageIndex]);
        UpdateUpgradeText(damageNextText, damageVisualValues[playerStats.damageIndex + 1]);
        UpdateUpgradeText(damageCostText, playerSO.GetDamageCosts()[playerStats.damageIndex]);

        //  LIFE STEAL
        UpdateUpgradeText(lifeStealCurrentText, lifeStealVisualValues[playerStats.lifeStealIndex]);
        UpdateUpgradeText(lifeStealNextText, lifeStealVisualValues[playerStats.lifeStealIndex + 1]);
        UpdateUpgradeText(lifeStealCostText, playerSO.GetLifeStealCosts()[playerStats.lifeStealIndex]);

        // MOVEMENT SPEED
        UpdateUpgradeText(movementSpeedCurrentText, movementSpeedVisualValues[playerStats.movementSpeedIndex]);
        UpdateUpgradeText(movementSpeedNextText, movementSpeedVisualValues[playerStats.movementSpeedIndex + 1]);
        UpdateUpgradeText(movementSpeedCostText, playerSO.GetMovementSpeedCosts()[playerStats.movementSpeedIndex]);

        //POWER UP DUR
        UpdateUpgradeText(powerupDurCurrentText, playerSO.GetPowerupDurValues()[playerStats.powerupDurIndex]);
        UpdateUpgradeText(powerupDurNextText, playerSO.GetPowerupDurValues()[playerStats.powerupDurIndex + 1]);
        UpdateUpgradeText(powerupDurCostText, playerSO.GetPowerupDurCosts()[playerStats.powerupDurIndex]);

        // MAX HEALTH
        UpdateUpgradeText(maxHealthCurrentText, playerSO.GetMaxHealthValues()[playerStats.maxHealthIndex]);
        UpdateUpgradeText(maxHealthNextText, playerSO.GetMaxHealthValues()[playerStats.maxHealthIndex + 1]);
        UpdateUpgradeText(maxHealthCostText, playerSO.GetMaxHealthCosts()[playerStats.maxHealthIndex]);

        // Dual Weapon 

        if (playerStats.isDualWeaponActive)
        {
            dualWeaponCostText.text = "MAX";
        }
        else
        {
            dualWeaponCostText.text = playerSO.GetDualWeaponCost().ToString();
        }

    }

    private void UpdateUpgradeText(TMP_Text textToUpdate, float value, string newString = null)
    {
        if (textToUpdate == null)
        {
            Debug.LogWarning("textToUpdate" + " is null");
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

    #region  Wave Related
    private void OnWaveCallClicked()
    {
        gameManager.OnWaveCalled();
        waveCallButton.gameObject.SetActive(false);
    }
    private void WaveCompleted()
    {
        waveCallButton.gameObject.SetActive(true);
        UpdateText(waveIndexText, playerStats.waveIndex, "Wave");
    }
    private void ConnectToSpawner()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnWaveCompleted += WaveCompleted;
    }
    #endregion

    #region  Update Texts - Text Related

    private void UpdateText(TMP_Text textToUpdate, int value, string newString = null)
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

    private void UpdateMoneyText()
    {
        UpdateText(moneyText, playerStats.money, "Money");
    }
    private void UpdateWaveIndexText()
    {
        UpdateText(waveIndexText, playerStats.waveIndex, "Wave");
    }
    private void UpdateXPText()
    {
        UpdateText(xpText, playerStats.experiencePoint, "XP");
    }
    #endregion  

}
