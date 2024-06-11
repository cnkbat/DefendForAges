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

    [Header("Panels")]
    [SerializeField] private List<GameObject> allPanels;
    [SerializeField] private GameObject joystickPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject waveWonPanel;
    [SerializeField] private GameObject gameLostPanel;

    [Header("GameHud Texts")]
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
    [SerializeField] TMP_Text playerKilledCountDownText;
    [SerializeField] private Button lateReviveButton;
    [SerializeField] private Button rewardedReviveButton;

    [Header("Power Up Slider")]
    [SerializeField] private Image powerUpFill;
    [SerializeField] private ParticleSystem powerUpVFX;

    [Header("Earnings")]
    [SerializeField] private Button normalApplyEarningsButton;
    [SerializeField] private Button rewardedApplyEarningsButton;
    [SerializeField] private GameObject waveWonOpenedChestIcon;
    [SerializeField] private GameObject waveWonClosedChestIcon;
    [SerializeField] private TMP_Text waveWoncollectedXPText;
    [SerializeField] private TMP_Text waveWoncollectedCoinText;
    [SerializeField] private TMP_Text waveWoncollectedMeatText;

    [Header("Game Lost Sequence")]
    [SerializeField] private GameObject gameLostOpenedChestIcon;
    [SerializeField] private GameObject gameLostClosedChestIcon;
    [SerializeField] private TMP_Text gameLostCollectedXPText;
    [SerializeField] private TMP_Text gameLostCollectedMeatText;
    [SerializeField] private TMP_Text gameLostCollectedCoinText;
    [SerializeField] private TMP_Text towerDestroyedCountDownText;
    [SerializeField] private Button loseGameButton;
    [SerializeField] private Button useGemToReviveTowerButton;

    #region Upgrading Variables

    [Header("---- Upgrading ----")]
    [SerializeField] private GameObject scrollPanel;

    [Header("Upgrade Buttons")]
    [SerializeField] private Button enableUpgradeHudButton;
    [SerializeField] private Button exitUpgradeHudButton;
    [SerializeField] private Button upgradeAttackSpeedButton;
    [SerializeField] private Button upgradeDamageButton;
    [SerializeField] private Button upgradeRangeButton;
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
    [SerializeField] private TMP_Text attackSpeedLevelText;

    [Header("Damage Texts")]
    [SerializeField] private TMP_Text damageCurrentText;
    [SerializeField] private TMP_Text damageNextText;
    [SerializeField] private TMP_Text damageCostText;
    [SerializeField] private TMP_Text damageLevelText;

    [Header("Range")]
    [SerializeField] private TMP_Text rangeCurrentText;
    [SerializeField] private TMP_Text rangeNextText;
    [SerializeField] private TMP_Text rangeCostText;
    [SerializeField] private TMP_Text rangeLevelText;

    [Header("Life Steal Texts")]
    [SerializeField] private TMP_Text lifeStealCurrentText;
    [SerializeField] private TMP_Text lifeStealNextText;
    [SerializeField] private TMP_Text lifeStealCostText;
    [SerializeField] private TMP_Text lifeStealLevelText;


    [Header("Movement Speed Texts")]
    [SerializeField] private TMP_Text movementSpeedCurrentText;
    [SerializeField] private TMP_Text movementSpeedNextText;
    [SerializeField] private TMP_Text movementSpeedCostText;
    [SerializeField] private TMP_Text movementSpeedLevelText;

    [Header("Powerup Dur Texts")]
    [SerializeField] private TMP_Text powerupDurCurrentText;
    [SerializeField] private TMP_Text powerupDurNextText;
    [SerializeField] private TMP_Text powerupDurCostText;
    [SerializeField] private TMP_Text powerupDurLevelText;


    [Header("Max Health Texts")]
    [SerializeField] private TMP_Text maxHealthCurrentText;
    [SerializeField] private TMP_Text maxHealthNextText;
    [SerializeField] private TMP_Text maxHealthCostText;
    [SerializeField] private TMP_Text maxHealthLevelText;


    [Header("Dual Weapon Texts")]
    [SerializeField] private TMP_Text dualWeaponCostText;

    [Header("!--- Upgrade Text Visuals ---!")]
    [SerializeField] private List<float> attackSpeedVisualValues;
    [SerializeField] private List<float> damageVisualValues;
    [SerializeField] private List<float> rangeVisualValues;
    [SerializeField] private List<float> lifeStealVisualValues;
    [SerializeField] private List<float> movementSpeedVisualValues;

    #endregion

    #region  On Enable - On Disable

    private void OnEnable()
    {

        playerStats = PlayerStats.instance;
        earningsHolder = EarningsHolder.instance;
        gameManager = GameManager.instance;

        playerStats.OnWaveWon += WaveCompleted;
        playerStats.OnWaveWon += EnableWaveWonUI;

        // Geçici olarak böyle yapildi (MVP'ye reklam entegrasyonu yetişirse)
        // onwavewondan kaldirip button aksiyonuna taşınacak ve wavewon ekranında
        // wavewon paneli açılacak
        // şuanda wave kazanılınca direkt olarak playera atıyor


        // Revive & Wave Control
        lateReviveButton.onClick.AddListener(OnLateReviveButtonClicked);
        rewardedReviveButton.onClick.AddListener(OnReviveButtonClicked);
        waveCallButton.onClick.AddListener(OnWaveCallClicked);
        normalApplyEarningsButton.onClick.AddListener(RegularDisableWaveWonUI);
        rewardedApplyEarningsButton.onClick.AddListener(RewardedDisableWaveWonUI);



        // Power UP
        playerStats.OnPowerUpValueChanged += UpdatePowerUpSliderValue;
        playerStats.OnPowerUpEnabled += EnablePowerupUIAnimation;
        playerStats.OnPowerUpDisabled += DisablePowerupUIAnimation;

        // Death & Revive
        playerStats.OnPlayerRevived += DeathReviveSequenceEnd;
        playerStats.OnPlayerKilled += DeathReviveSequence;

        // Text Updates
        playerStats.OnMoneyChange += UpdateMoneyText;
        playerStats.OnMeatChange += UpdateMeatText;
        playerStats.OnExperienceGain += UpdateLevelBar;
        earningsHolder.OnEarningsApply += ActivateAndUpdateWaveWonEarningsTexts;
        earningsHolder.OnEarningsApply += ActivateAndUpdateTowerDeathEarningsTexts;

        // Lose Game
        for (int i = 0; i < gameManager.allCities.Count; i++)
        {
            gameManager.allCities[i].GetTower().OnTargetDestroyed += GameLostPanelSequence;
        }

        loseGameButton.onClick.AddListener(GameLostPanelSequenceEnd);


        #region  Upgrading
        //upgrade button events

        enableUpgradeHudButton.onClick.AddListener(EnableUpgradeHud);
        exitUpgradeHudButton.onClick.AddListener(DisableUpgradeHud);
        upgradeAttackSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeAttackSpeed);
        upgradeDamageButton.onClick.AddListener(playerStats.AttemptUpgradeDamage);
        upgradeRangeButton.onClick.AddListener(playerStats.AttemptUpgradeRange);
        upgradeLifeStealButton.onClick.AddListener(playerStats.AttemptUpgradeLifeSteal);
        upgradeMovementSpeedButton.onClick.AddListener(playerStats.AttemptUpgradeMovementSpeed);
        upgradePowerupDurButton.onClick.AddListener(playerStats.AttemptUpgradePowerupDuration);
        upgradeMaxHealthButton.onClick.AddListener(playerStats.AttemptUpgradeMaxHealth);
        upgradeDualWeaponButton.onClick.AddListener(playerStats.AttemptUpgradeDualWeapon);

        // Upgrade Done Events Assign
        playerStats.OnAttackSpeedUpgraded += UpdateAttackSpeedTexts;
        playerStats.OnDamageUpgraded += UpdateDamageTexts;
        playerStats.OnRangeUpgraded += UpdateRangeTexts;
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
        rewardedReviveButton.onClick.RemoveAllListeners();
        waveCallButton.onClick.RemoveAllListeners();
        normalApplyEarningsButton.onClick.RemoveAllListeners();

        playerStats.OnWaveWon -= EnableWaveWonUI;
        playerStats.OnWaveWon -= WaveCompleted;


        // Power UP
        playerStats.OnPowerUpValueChanged += UpdatePowerUpSliderValue;
        playerStats.OnPowerUpEnabled += EnablePowerupUIAnimation;
        playerStats.OnPowerUpDisabled += DisablePowerupUIAnimation;

        // Death & Revive
        playerStats.OnPlayerRevived -= DeathReviveSequenceEnd;
        playerStats.OnPlayerKilled -= DeathReviveSequence;

        // Text Updates 
        playerStats.OnMoneyChange -= UpdateMoneyText;
        playerStats.OnMeatChange -= UpdateMeatText;
        playerStats.OnExperienceGain -= UpdateLevelBar;
        earningsHolder.OnEarningsApply -= ActivateAndUpdateWaveWonEarningsTexts;
        earningsHolder.OnEarningsApply -= ActivateAndUpdateTowerDeathEarningsTexts;

        // Lose Game
        for (int i = 0; i < gameManager.allCities.Count; i++)
        {
            gameManager.allCities[i].GetTower().OnTargetDestroyed -= GameLostPanelSequence;
        }

        loseGameButton.onClick.RemoveAllListeners();


        #region  Upgrading

        // Upgrading Button Events Clean Up
        enableUpgradeHudButton.onClick.RemoveAllListeners();
        exitUpgradeHudButton.onClick.RemoveAllListeners();
        upgradeAttackSpeedButton.onClick.RemoveAllListeners();
        upgradeDamageButton.onClick.RemoveAllListeners();
        upgradeRangeButton.onClick.RemoveAllListeners();
        upgradeLifeStealButton.onClick.RemoveAllListeners();
        upgradeMovementSpeedButton.onClick.RemoveAllListeners();
        upgradePowerupDurButton.onClick.RemoveAllListeners();
        upgradeMaxHealthButton.onClick.RemoveAllListeners();


        // Upgrade Done Events Clean Up
        playerStats.OnAttackSpeedUpgraded -= UpdateAttackSpeedTexts;
        playerStats.OnDamageUpgraded -= UpdateDamageTexts;
        playerStats.OnRangeUpgraded -= UpdateRangeTexts;

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
        DisableUpgradingButton();
        SetStartingUI();
    }
    private void SetStartingUI()
    {
        GetBackToGamePanel();

        UpdatePowerUpSliderValue(0);
        UpdateMoneyText();
        UpdateMeatText();

        playerSO = playerStats.GetPlayerSO();
        UpdateUpgradeUI();

        UpdateAllWavesProgressBar();
    }

    #endregion

    #region Upgrade


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
        if (playerStats.dualWeaponIndex > 0)
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
        UpdateUpgradeText(maxHealthLevelText, playerStats.maxHealthIndex + 1);
    }

    //POWER UP DUR
    private void UpdatePowerupDurTexts()
    {
        UpdateUpgradeText(powerupDurCurrentText, playerSO.GetPowerupDurValues()[playerStats.powerupDurIndex]);
        UpdateUpgradeText(powerupDurNextText, playerSO.GetPowerupDurValues()[playerStats.powerupDurIndex + 1]);
        UpdateUpgradeText(powerupDurCostText, playerSO.GetPowerupDurCosts()[playerStats.powerupDurIndex]);
        UpdateUpgradeText(powerupDurLevelText, playerStats.powerupDurIndex + 1);
    }

    // MOVEMENT SPEED
    private void UpdateMovementSpeedTexts()
    {
        UpdateUpgradeText(movementSpeedCurrentText, movementSpeedVisualValues[playerStats.movementSpeedIndex]);
        UpdateUpgradeText(movementSpeedNextText, movementSpeedVisualValues[playerStats.movementSpeedIndex + 1]);
        UpdateUpgradeText(movementSpeedCostText, playerSO.GetMovementSpeedCosts()[playerStats.movementSpeedIndex]);
        UpdateUpgradeText(movementSpeedLevelText, playerStats.movementSpeedIndex + 1);
    }

    //  LIFE STEAL
    private void UpdateLifeStealTexts()
    {
        UpdateUpgradeText(lifeStealCurrentText, lifeStealVisualValues[playerStats.lifeStealIndex]);
        UpdateUpgradeText(lifeStealNextText, lifeStealVisualValues[playerStats.lifeStealIndex + 1]);
        UpdateUpgradeText(lifeStealCostText, playerSO.GetLifeStealCosts()[playerStats.lifeStealIndex]);
        UpdateUpgradeText(lifeStealLevelText, playerStats.lifeStealIndex + 1);
    }

    // DAMAGE
    private void UpdateDamageTexts()
    {
        UpdateUpgradeText(damageCurrentText, damageVisualValues[playerStats.damageIndex]);
        UpdateUpgradeText(damageNextText, damageVisualValues[playerStats.damageIndex + 1]);
        UpdateUpgradeText(damageCostText, playerSO.GetDamageCosts()[playerStats.damageIndex]);
        UpdateUpgradeText(damageLevelText, playerStats.damageIndex + 1);
    }
    private void UpdateRangeTexts()
    {
        UpdateUpgradeText(rangeCurrentText, rangeVisualValues[playerStats.rangeIndex]);
        UpdateUpgradeText(rangeNextText, rangeVisualValues[playerStats.rangeIndex + 1]);
        UpdateUpgradeText(rangeCostText, playerSO.GetRangeCosts()[playerStats.rangeIndex]);
        UpdateUpgradeText(rangeLevelText, playerStats.rangeIndex + 1);
    }

    // ATTACK SPEED
    private void UpdateAttackSpeedTexts()
    {
        UpdateUpgradeText(attackSpeedCurrentText, attackSpeedVisualValues[playerStats.attackSpeedIndex]);
        UpdateUpgradeText(attackSpeedNextText, attackSpeedVisualValues[playerStats.attackSpeedIndex + 1]);
        UpdateUpgradeText(attackSpeedCostText, playerSO.GetAttackSpeedCosts()[playerStats.attackSpeedIndex]);
        UpdateUpgradeText(attackSpeedLevelText, playerStats.attackSpeedIndex + 1);
    }


    #endregion

    #region Revive

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
        if (gameManager.isAttackPhase) return;

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
    public void OnApplyEarningToPlayerButtonClicked(float newMultipiler)
    {
        earningsHolder.OnBonusMultiplierApplied?.Invoke(newMultipiler);
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


    #region Panel Management

    public void DeactivateAllPanels()
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            allPanels[i].SetActive(false);
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
    private void GetBackToGamePanel()
    {
        DeactivateAllPanels();

        ActivatePanel(gamePanel, joystickPanel);
    }

    #region Upgrade Panel Management

    private void EnableUpgradeHud()
    {
        ActivatePanel(upgradePanel);
    }

    private void DisableUpgradeHud()
    {
        DeactivatePanel(upgradePanel);
        scrollPanel.transform.localPosition = Vector3.zero;
    }

    #endregion

    #region  Death Panel Management
    private void DeathReviveSequence()
    {
        DeactivateAllPanels();
        ActivatePanel(deathPanel);

        lateReviveButton.gameObject.SetActive(false);

        StartCoroutine(PlayerDeathCountDown());
    }
    IEnumerator PlayerDeathCountDown()
    {
        if (!deathPanel.activeSelf) yield return null;

        playerKilledCountDownText.text = "5";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        playerKilledCountDownText.text = "4";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        playerKilledCountDownText.text = "3";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        playerKilledCountDownText.text = "2";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        playerKilledCountDownText.text = "1";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        lateReviveButton.gameObject.SetActive(true);
        playerKilledCountDownText.gameObject.SetActive(false);
    }
    private void DeathReviveSequenceEnd()
    {
        GetBackToGamePanel();
    }



    #endregion

    #region  Lose Panel Management
    private void GameLostPanelSequence()
    {
        DeactivateAllPanels();
        ActivatePanel(gameLostPanel);

        gameManager.isGameFreezed = true;

        loseGameButton.gameObject.SetActive(false);

        gameLostOpenedChestIcon.SetActive(false);
        gameLostClosedChestIcon.SetActive(true);

        gameLostCollectedXPText.transform.parent.gameObject.SetActive(false);
        gameLostCollectedCoinText.transform.parent.gameObject.SetActive(false);
        gameLostCollectedMeatText.transform.parent.gameObject.SetActive(false);


        StartCoroutine(TowerDeathCountDown());
    }

    IEnumerator TowerDeathCountDown()
    {
        if (!deathPanel.activeSelf) yield return null;

        towerDestroyedCountDownText.text = "3";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        towerDestroyedCountDownText.text = "2";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        towerDestroyedCountDownText.text = "1";
        yield return new WaitForSeconds(1);

        if (!deathPanel.activeSelf) yield return null;

        loseGameButton.gameObject.SetActive(true);
        towerDestroyedCountDownText.gameObject.SetActive(false);
    }

    private void GameLostPanelSequenceEnd()
    {
        StartCoroutine(DisableGameLostUI());

        OnApplyEarningToPlayerButtonClicked(1);

        loseGameButton.gameObject.SetActive(false);
        useGemToReviveTowerButton.gameObject.SetActive(false);

        gameLostOpenedChestIcon.SetActive(true);
        gameLostClosedChestIcon.SetActive(false);

        gameLostCollectedXPText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedCoinText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedMeatText.transform.parent.gameObject.SetActive(true);
    }

    IEnumerator DisableGameLostUI()
    {
        yield return new WaitForSeconds(2);

        GetBackToGamePanel();
        gameManager.LevelLost();
    }

    private void ActivateAndUpdateTowerDeathEarningsTexts(int meatValue, int coinValue, int xpValue)
    {

        gameLostOpenedChestIcon.SetActive(true);
        gameLostClosedChestIcon.SetActive(false);


        gameLostCollectedXPText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedCoinText.transform.parent.gameObject.SetActive(true);
        gameLostCollectedMeatText.transform.parent.gameObject.SetActive(true);

        gameLostCollectedXPText.text = xpValue.ToString();
        gameLostCollectedCoinText.text = coinValue.ToString();
        gameLostCollectedMeatText.text = meatValue.ToString();

    }

    #endregion

    #region Wave Won Management

    public void EnableWaveWonUI()
    {
        ActivatePanel(waveWonPanel);

        waveWonOpenedChestIcon.SetActive(false);
        waveWonClosedChestIcon.SetActive(true);

        waveWoncollectedXPText.transform.parent.gameObject.SetActive(false);
        waveWoncollectedCoinText.transform.parent.gameObject.SetActive(false);
        waveWoncollectedMeatText.transform.parent.gameObject.SetActive(false);

    }
    IEnumerator DisableWaveWonUI()
    {
        yield return new WaitForSeconds(2);
        GetBackToGamePanel();
        gameManager.OnApplyEarnings?.Invoke();
    }

    private void ActivateAndUpdateWaveWonEarningsTexts(int meatValue, int coinValue, int xpValue)
    {
        waveWonOpenedChestIcon.SetActive(true);
        waveWonClosedChestIcon.SetActive(false);

        waveWoncollectedXPText.transform.parent.gameObject.SetActive(true);
        waveWoncollectedCoinText.transform.parent.gameObject.SetActive(true);
        waveWoncollectedMeatText.transform.parent.gameObject.SetActive(true);

        waveWoncollectedXPText.text = xpValue.ToString();
        waveWoncollectedCoinText.text = coinValue.ToString();
        waveWoncollectedMeatText.text = meatValue.ToString();

    }

    public void RewardedDisableWaveWonUI()
    {
        OnApplyEarningToPlayerButtonClicked(2);

        StartCoroutine(DisableWaveWonUI());
    }
    public void RegularDisableWaveWonUI()
    {
        OnApplyEarningToPlayerButtonClicked(1);

        StartCoroutine(DisableWaveWonUI());
    }



    #endregion

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
