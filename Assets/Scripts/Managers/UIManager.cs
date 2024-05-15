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

    [Header("GameHud Texts")]
    [SerializeField] private GameObject gameHud;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text meatText;
    [SerializeField] private TMP_Text waveIndexText;

    [Header("Wave Control")]
    [SerializeField] private Button waveCallButton;
    [SerializeField] private GameObject allWavesProgressBar;
    [SerializeField] private List<GameObject> allWavesProgressBarImages;
    [SerializeField] Slider inwaveProgressBar;

    [Header("Revive")]
    [SerializeField] private Button reviveButton;
    [SerializeField] private GameObject reviveUI;

    #region Upgrading Variables

    [Header("---- Upgrading ----")]

    [Header("Upgrade Buttons")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button enableUpgradeHudButton;
    [SerializeField] private Button exitUpgradeHudButton;
    [SerializeField] private Button upgradeAttackSpeedButton;
    [SerializeField] private Button upgradeDamageButton;
    [SerializeField] private Button upgradeLifeStealButton;
    [SerializeField] private Button upgradeMovementSpeedButton;
    [SerializeField] private Button upgradePowerupDurButton;
    [SerializeField] private Button upgradeMaxHealthButton;
    [SerializeField] private Button upgradeDualWeaponButton;
    #endregion

    #region Upgrading Texts Variables

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

    #endregion

    #region  On Enable - On Disable

    private void OnEnable()
    {

        playerStats = PlayerStats.instance;

        playerStats.OnWaveWon += WaveCompleted;

        // Revive & Wave Control
        reviveButton.onClick.AddListener(OnReviveButtonPressed);
        waveCallButton.onClick.AddListener(OnWaveCallClicked);


        // Text Updates
        playerStats.OnExperiencePointChange += UpdateXPText;
        playerStats.OnMoneyChange += UpdateMoneyText;
        playerStats.OnMeatChange += UpdateMeatText;


        #region  Upgrading
        //upgrade button events

        enableUpgradeHudButton.onClick.AddListener(EnableUpgradeHud);
        exitUpgradeHudButton.onClick.AddListener(DisableUpgradeHud);
        upgradeAttackSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeAttackSpeed);
        upgradeDamageButton.onClick.AddListener(playerStats.AttemptUpgradeDamage);
        upgradeLifeStealButton.onClick.AddListener(playerStats.AttemptUpgradeLifeSteal);
        upgradeMovementSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeMovementSpeed);
        upgradePowerupDurButton.onClick.AddListener(playerStats.AttemptUpgradePowerupDuration);
        upgradeMaxHealthButton.onClick.AddListener(playerStats.AttemptUpgradeMaxHealth);
        upgradeDualWeaponButton.onClick.AddListener(playerStats.AttemptUpgradeDualWeapon);

        // Upgrade Done Events Assign
        playerStats.OnAttackSpeedUpgraded += UpdateAttackSpeedTexts;
        playerStats.OnDamageUpgraded += UpdateDamageTexts;
        playerStats.OnLifeStealUpgraded += UpdateLifeStealTexts;
        playerStats.OnMovementSpeedUpgraded += UpdateMovementSpeedTexts;
        playerStats.OnPowerupDurUpgraded += UpdatePowerupDurTexts;
        playerStats.OnMaxHealthUpgraded += UpdateMaxHealthTexts;
        playerStats.OnDualWeaponUpgraded += UpdateDualWeaponTexts;

        #endregion

    }

    private void OnDisable()
    {
        // Revive & Wave Control
        reviveButton.onClick.RemoveAllListeners();
        waveCallButton.onClick.RemoveAllListeners();
        playerStats.OnWaveWon -= WaveCompleted;

        // Text Updates
        playerStats.OnExperiencePointChange -= UpdateXPText;
        playerStats.OnMoneyChange -= UpdateMoneyText;


        #region  Upgrading

        // Upgrading Button Events Clean Up
        enableUpgradeHudButton.onClick.RemoveAllListeners();
        exitUpgradeHudButton.onClick.RemoveAllListeners();
        upgradeAttackSpeedButton.onClick.RemoveAllListeners();
        upgradeDamageButton.onClick.RemoveAllListeners();
        upgradeLifeStealButton.onClick.RemoveAllListeners();
        upgradeMovementSpeedButton.onClick.RemoveAllListeners();
        upgradePowerupDurButton.onClick.RemoveAllListeners();
        upgradeMaxHealthButton.onClick.RemoveAllListeners();


        // Upgrade Done Events Clean Up
        playerStats.OnAttackSpeedUpgraded -= UpdateAttackSpeedTexts;
        playerStats.OnDamageUpgraded -= UpdateDamageTexts;
        playerStats.OnLifeStealUpgraded -= UpdateLifeStealTexts;
        playerStats.OnMovementSpeedUpgraded -= UpdateMovementSpeedTexts;
        playerStats.OnPowerupDurUpgraded -= UpdatePowerupDurTexts;
        playerStats.OnMaxHealthUpgraded -= UpdateMaxHealthTexts;
        playerStats.OnDualWeaponUpgraded -= UpdateDualWeaponTexts;

        #endregion

    }

    #endregion

    #region  Start
    private void Start()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        DisableUpgradingButton();
        SetStartingUI();
    }
    private void SetStartingUI()
    {
        UpdateWaveIndexText();
        UpdateMoneyText();
        UpdateXPText();
        UpdateMeatText();

        playerSO = playerStats.GetPlayerSO();
        UpdateUpgradeUI();

        UpdateAllWavesProgressBar();
    }

    #endregion

    #region Upgrade

    private void EnableUpgradeHud()
    {
        upgradePanel.SetActive(true);
    }

    private void DisableUpgradeHud()
    {
        upgradePanel.SetActive(false);
    }

    private void UpdateUpgradeUI()
    {
        // ATTACK SPEED
        UpdateAttackSpeedTexts();

        // DAMAGE
        UpdateDamageTexts();

        //  LIFE STEAL
        UpdateLifeStealTexts();

        // MOVEMENT SPEED
        UpdateMovementSpeedTexts();

        //POWER UP DUR
        UpdatePowerupDurTexts();

        // MAX HEALTH
        UpdateMaxHealthTexts();

        // Dual Weapon 
        UpdateDualWeaponTexts();

    }

    // Dual Weapon 
    private void UpdateDualWeaponTexts()
    {
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

    // MAX HEALTH
    private void UpdateMaxHealthTexts()
    {
        UpdateUpgradeText(maxHealthCurrentText, playerSO.GetMaxHealthValues()[playerStats.maxHealthIndex]);
        UpdateUpgradeText(maxHealthNextText, playerSO.GetMaxHealthValues()[playerStats.maxHealthIndex + 1]);
        UpdateUpgradeText(maxHealthCostText, playerSO.GetMaxHealthCosts()[playerStats.maxHealthIndex]);
    }

    //POWER UP DUR
    private void UpdatePowerupDurTexts()
    {
        UpdateUpgradeText(powerupDurCurrentText, playerSO.GetPowerupDurValues()[playerStats.powerupDurIndex]);
        UpdateUpgradeText(powerupDurNextText, playerSO.GetPowerupDurValues()[playerStats.powerupDurIndex + 1]);
        UpdateUpgradeText(powerupDurCostText, playerSO.GetPowerupDurCosts()[playerStats.powerupDurIndex]);
    }

    // MOVEMENT SPEED
    private void UpdateMovementSpeedTexts()
    {
        UpdateUpgradeText(movementSpeedCurrentText, movementSpeedVisualValues[playerStats.movementSpeedIndex]);
        UpdateUpgradeText(movementSpeedNextText, movementSpeedVisualValues[playerStats.movementSpeedIndex + 1]);
        UpdateUpgradeText(movementSpeedCostText, playerSO.GetMovementSpeedCosts()[playerStats.movementSpeedIndex]);
    }

    //  LIFE STEAL
    private void UpdateLifeStealTexts()
    {
        UpdateUpgradeText(lifeStealCurrentText, lifeStealVisualValues[playerStats.lifeStealIndex]);
        UpdateUpgradeText(lifeStealNextText, lifeStealVisualValues[playerStats.lifeStealIndex + 1]);
        UpdateUpgradeText(lifeStealCostText, playerSO.GetLifeStealCosts()[playerStats.lifeStealIndex]);
    }

    // DAMAGE
    private void UpdateDamageTexts()
    {
        UpdateUpgradeText(damageCurrentText, damageVisualValues[playerStats.damageIndex]);
        UpdateUpgradeText(damageNextText, damageVisualValues[playerStats.damageIndex + 1]);
        UpdateUpgradeText(damageCostText, playerSO.GetDamageCosts()[playerStats.damageIndex]);
    }

    // ATTACK SPEED
    private void UpdateAttackSpeedTexts()
    {
        UpdateUpgradeText(attackSpeedCurrentText, attackSpeedVisualValues[playerStats.attackSpeedIndex]);
        UpdateUpgradeText(attackSpeedNextText, attackSpeedVisualValues[playerStats.attackSpeedIndex + 1]);
        UpdateUpgradeText(attackSpeedCostText, playerSO.GetAttackSpeedCosts()[playerStats.attackSpeedIndex]);
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
        DisableUpgradingButton();
        DisableUpgradeHud();

        waveCallButton.gameObject.SetActive(false);

        allWavesProgressBar.SetActive(false);

        inwaveProgressBar.gameObject.SetActive(true);

    }
    public void UpdateInWaveProgressBarValue(float value)
    {
        inwaveProgressBar.value = value;
    }

    private void WaveCompleted()
    {
        waveCallButton.gameObject.SetActive(true);
        UpdateText(waveIndexText, playerStats.waveIndex, "Wave");

        inwaveProgressBar.gameObject.SetActive(false);
        UpdateAllWavesProgressBar();
    }

    private void UpdateAllWavesProgressBar()
    {
        allWavesProgressBar.SetActive(true);

        for (int i = 0; i < allWavesProgressBarImages.Count; i++)
        {
            allWavesProgressBarImages[i].SetActive(false);
        }

        for (int i = 0; i < playerStats.GetWaveIndex(); i++)
        {
            allWavesProgressBarImages[i].SetActive(true);
        }
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
    private void UpdateMeatText()
    {
        UpdateText(meatText, playerStats.meat, "Meat");
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

}
