using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private RPGSystemSO playerSO;
    private PlayerStats playerStats;
    private GameManager gameManager;
    private EarningsHolder earningsHolder;

    [Header("GameHud Texts")]
    [SerializeField] private GameObject gameHud;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text meatText;
    [SerializeField] private TMP_Text currentWaveIndexText;
    [SerializeField] private TMP_Text nextWaveIndexText;

    [Header("Wave Control")]
    [SerializeField] private Button waveCallButton;
    [SerializeField] private List<GameObject> totalProgressBarImages;
    [SerializeField] private Slider totalWaveProgressBar;
    [SerializeField] private Slider inwaveProgressBar;

    [Header("ProgressBar Animation")]
    [SerializeField] private RectTransform progressBarHideTransform;
    [SerializeField] private RectTransform totalWaveProgressBarOriginalTransform;
    [SerializeField] private RectTransform inwaveProgressBarOriginalTransform;
    [SerializeField] private float progressBarAnimDur;
    [SerializeField] private Ease progressBarEaseType;


    [Header("Revive")]
    [SerializeField] private Button lateReviveButton;
    [SerializeField] private Button reviveButton;
    [SerializeField] private GameObject reviveUI;

    [Header("Power Up Slider")]
    [SerializeField] private Image powerUpFill;
    [SerializeField] private ParticleSystem powerUpVFX;

    [Header("Earnings")]
    [SerializeField] private Button normalApplyEarningsButton;


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

    [Header("Levelling UI")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] Slider levelSlider;

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
        earningsHolder = EarningsHolder.instance;

        playerStats.OnWaveWon += WaveCompleted;


        // Geçici olarak böyle yapildi (MVP'ye reklam entegrasyonu yetişirse)
        // onwavewondan kaldirip button aksiyonuna taşınacak ve wavewon ekranında
        // wavewon paneli açılacak
        // şuanda wave kazanılınca direkt olarak playera atıyor

        playerStats.OnWaveWon += OnApplyEarningToPlayerButtonClicked;

        // Revive & Wave Control
        lateReviveButton.onClick.AddListener(OnLateReviveButtonClicked);
        reviveButton.onClick.AddListener(OnReviveButtonClicked);
        waveCallButton.onClick.AddListener(OnWaveCallClicked);
        normalApplyEarningsButton.onClick.AddListener(OnApplyEarningToPlayerButtonClicked);

        // Power UP
        playerStats.OnPowerUpValueChanged += UpdatePowerUpSliderValue;
        playerStats.OnPowerUpEnabled += EnablePowerupUIAnimation;
        playerStats.OnPowerUpDisabled += DisablePowerupUIAnimation;

        // Death & Revive
        playerStats.OnPlayerRevived += DisableReviveUI;
        playerStats.OnPlayerKilled += EnableReviveUI;

        // Text Updates
        playerStats.OnMoneyChange += UpdateMoneyText;
        playerStats.OnMeatChange += UpdateMeatText;
        playerStats.OnExperienceGain += UpdateLevelBar;


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
        // Revive & Wave Control Buttons
        lateReviveButton.onClick.RemoveAllListeners();
        reviveButton.onClick.RemoveAllListeners();
        waveCallButton.onClick.RemoveAllListeners();
        normalApplyEarningsButton.onClick.RemoveAllListeners();

        playerStats.OnWaveWon -= WaveCompleted;
        playerStats.OnWaveWon -= OnApplyEarningToPlayerButtonClicked;

        // Power UP
        playerStats.OnPowerUpValueChanged += UpdatePowerUpSliderValue;
        playerStats.OnPowerUpEnabled += EnablePowerupUIAnimation;
        playerStats.OnPowerUpDisabled += DisablePowerupUIAnimation;

        // Death & Revive
        playerStats.OnPlayerRevived -= DisableReviveUI;
        playerStats.OnPlayerKilled -= EnableReviveUI;

        // Text Updates 
        playerStats.OnMoneyChange -= UpdateMoneyText;
        playerStats.OnMeatChange -= UpdateMeatText;
        playerStats.OnExperienceGain -= UpdateLevelBar;


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
        UpdatePowerUpSliderValue(0);
        UpdateMoneyText();
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


    public void EnableReviveUI()
    {
        reviveUI.SetActive(true);
    }
    public void DisableReviveUI()
    {
        reviveUI.SetActive(false);
    }

    private void OnLateReviveButtonClicked()
    {
        playerStats.OnLateReviveButtonClicked?.Invoke();
        playerStats.OnPlayerRevived?.Invoke();
    }
    private void OnReviveButtonClicked()
    {
        playerStats.OnReviveButtonClicked?.Invoke();
    }

    #endregion

    #region  Wave Related
    private void OnWaveCallClicked()
    {
        gameManager.OnWaveCalled();
        DisableUpgradingButton();
        DisableUpgradeHud();

        waveCallButton.gameObject.SetActive(false);

        totalWaveProgressBar.transform.parent.DOLocalMoveY(progressBarHideTransform.anchoredPosition.y, progressBarAnimDur).
            SetEase(progressBarEaseType).
                OnComplete(() => totalWaveProgressBar.transform.parent.gameObject.SetActive(false));


        inwaveProgressBar.transform.parent.gameObject.SetActive(true);

        inwaveProgressBar.transform.parent.DOLocalMoveY(inwaveProgressBarOriginalTransform.anchoredPosition.y, progressBarAnimDur)
            .SetEase(progressBarEaseType);

        UpdateInWaveProgressBarTexts();
    }

    public void UpdateInWaveProgressBarValue(float value)
    {
        inwaveProgressBar.value = value;
    }


    private void WaveCompleted()
    {
        Debug.Log("work");
        waveCallButton.gameObject.SetActive(true);
        UpdateAllWavesProgressBar();
    }

    private void UpdateAllWavesProgressBar()
    {
        inwaveProgressBar.transform.parent.DOLocalMoveY(progressBarHideTransform.anchoredPosition.y, progressBarAnimDur).
           SetEase(progressBarEaseType).
               OnComplete(() => inwaveProgressBar.transform.parent.gameObject.SetActive(false));

        totalWaveProgressBar.transform.parent.gameObject.SetActive(true);
        totalWaveProgressBar.transform.parent.DOLocalMoveY(totalWaveProgressBarOriginalTransform.anchoredPosition.y, progressBarAnimDur)
            .SetEase(progressBarEaseType);
        totalWaveProgressBar.value = (float)playerStats.GetWaveIndex() / (float)gameManager.totalWaveCount;

        for (int i = 0; i < totalProgressBarImages.Count; i++)
        {
            totalProgressBarImages[i].SetActive(false);
        }

        Debug.Log("city Index =" + playerStats.GetCityIndex());

        totalProgressBarImages[playerStats.GetCityIndex()].SetActive(true);
        UpdateInWaveProgressBarTexts();

    }

    public void UpdateInWaveProgressBarTexts()
    {
        UpdateText(currentWaveIndexText, playerStats.GetWaveIndex());
        UpdateText(nextWaveIndexText, playerStats.GetWaveIndex() + 1);
    }

    #endregion

    #region Power Up
    public void UpdatePowerUpSliderValue(float newValue)
    {
        powerUpFill.fillAmount = newValue;
    }

    public void EnablePowerupUIAnimation()
    {
        powerUpFill.fillAmount = 1;
        powerUpVFX.Play();
    }

    public void DisablePowerupUIAnimation()
    {
        powerUpFill.fillAmount = 0;
        powerUpVFX.Stop();
    }
    #endregion

    #region Earnings
    public void OnApplyEarningToPlayerButtonClicked()
    {
        earningsHolder.OnBonusMultiplierApplied?.Invoke(1);
        gameManager.OnApplyEarnings?.Invoke();
    }
    #endregion

    #region LevelBar
    public void UpdateLevelBar(int playerLevel, float sliderLevel, int currentXP, int nextXP)
    {
        levelSlider.value = sliderLevel;
        UpdateText(levelText, playerLevel);
        xpText.text = currentXP.ToString() + " / " + nextXP.ToString();
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
        UpdateText(moneyText, playerStats.money);
    }
    private void UpdateMeatText()
    {
        UpdateText(meatText, playerStats.meat);
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
