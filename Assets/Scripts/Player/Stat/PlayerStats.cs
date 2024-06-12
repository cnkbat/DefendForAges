using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerStats : Singleton<PlayerStats>
{
    SaveManager saveManager;

    GameManager gameManager;

    [Header("Scriptable Object")]
    [SerializeField] private RPGSystemSO rpgSystemSO;

    [Header("Saved Indexes")]
    [HideInInspector] public int playerLevel;
    [HideInInspector] public int money;
    [HideInInspector] public int experiencePoint;
    [HideInInspector] public int meat;
    [HideInInspector] public int damageIndex;
    [HideInInspector] public int attackSpeedIndex;
    [HideInInspector] public int rangeIndex;
    [HideInInspector] public int movementSpeedIndex;
    [HideInInspector] public int powerupDurIndex;
    [HideInInspector] public int lifeStealIndex;
    [HideInInspector] public int maxHealthIndex;
    [HideInInspector] public int dualWeaponIndex;


    [HideInInspector] public int waveIndex;

    [Header("City & Era Saves")]
    [HideInInspector] public int cityIndex;

    [Header("Ingame Values")]
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float range;
    [SerializeField] private float powerupDur;
    [SerializeField] private float lifeStealRate;
    [SerializeField] private float maxHealth;

    [Header("Death")]
    public PlayerDeathHandler playerDeathHandler;

    [Header("Power Up")]
    [SerializeField] private float maxPowerUpFillValue;
    [Tooltip("Kaç sonraki geliştirme gücünde olmasini istiyorsak o değer girilecek.")][SerializeField] private int powerUpUpgradeIndexValue;
    private bool isPowerupEnabled;
    private float currentPowerUpValue;

    [Header("-------Stat Change Events ------")]
    public Action<float> OnKillEnemy;

    [Header("Upgrade Events")]
    public Action OnAttackSpeedUpgraded;
    public Action OnDamageUpgraded;
    public Action OnRangeUpgraded;
    public Action OnLifeStealUpgraded;
    public Action OnMovementSpeedUpgraded;
    public Action OnPowerupDurUpgraded;
    public Action OnMaxHealthUpgraded;
    public Action OnDualWeaponUpgraded;

    [Header("Action Events")]
    public Action OnPlayerRevived;
    public Action OnWaveWon;
    public Action OnPlayerKilled;
    public Action<int> OnWeaponActivision;
    public Action OnLifeStolen;
    public Action OnRangeSet;

    [Header("UI Events")]
    public Action OnExperiencePointChange;
    public Action<int, float, int, int> OnExperienceGain;
    public Action OnMoneyChange;
    public Action OnMeatChange;
    public Action OnReviveButtonClicked;
    public Action OnLateReviveButtonClicked;

    [Header("UI Power Up")]
    public Action<float> OnPowerUpValueChanged;
    public Action OnPowerUpEnabled;
    public Action OnPowerUpDisabled;

    protected override void Awake()
    {
        LoadPlayerData();
    }

    #region  OnEnable / OnDisable
    private void OnEnable()
    {
        saveManager = SaveManager.instance;

        if (saveManager != null)
        {
            saveManager.OnSaved += SavePlayerData;
            saveManager.OnResetData += ResetData;
        }

        OnKillEnemy += EarnBonusOnKill;
        OnPlayerRevived += FillCurrentHealth;
        OnLateReviveButtonClicked += SetWaveSystemBackToCheckpoint;
    }

    private void OnDisable()
    {
        saveManager = SaveManager.instance;

        if (saveManager != null)
        {
            saveManager.OnSaved -= SavePlayerData;

            saveManager.OnResetData -= ResetData;
        }

        OnKillEnemy -= EarnBonusOnKill;
        OnPlayerRevived -= FillCurrentHealth;
        OnLateReviveButtonClicked -= SetWaveSystemBackToCheckpoint;
    }
    #endregion

    private void Start()
    {
        saveManager = SaveManager.instance;
        gameManager = GameManager.instance;

        playerDeathHandler = GetComponent<PlayerDeathHandler>();
        FillCurrentHealth();
        LookForDualWeapon();

        transform.position = gameManager.allCities[cityIndex].GetStartPoint();


    }

    #region Upgrading

    private void AttemptUpgradeStat(int indexToUpgrade, List<int> cost, CurrencyType currencyType, RPGUpgradesType upgradeType = RPGUpgradesType.empty)
    {
        if (indexToUpgrade + 1 >= cost.Count) return; // geçici max sistemi tex değişecek falan filan.

        if (currencyType == CurrencyType.money)
        {
            if (DecrementMoney(cost[indexToUpgrade]))
            {
                UpgradeSuccesful(upgradeType);
            }
            else
            {
                // pop up offer
            }
        }
        else if (currencyType == CurrencyType.meat)
        {
            if (DecrementMeat(cost[indexToUpgrade]))
            {
                UpgradeSuccesful(upgradeType);
            }
            else
            {
                // pop up offer
            }
        }

    }

    private void UpgradeSuccesful(RPGUpgradesType upgradesType)
    {
        if (upgradesType == RPGUpgradesType.empty) return;
        HandleUpgrade(upgradesType);
        UpdateStats();
    }

    private void HandleUpgrade(RPGUpgradesType upgradesType)
    {
        switch (upgradesType)
        {
            case RPGUpgradesType.attackSpeed:
                attackSpeedIndex++;
                OnAttackSpeedUpgraded?.Invoke();
                break;
            case RPGUpgradesType.damage:
                damageIndex++;
                OnDamageUpgraded?.Invoke();
                break;
            case RPGUpgradesType.range:
                rangeIndex++;
                OnRangeUpgraded?.Invoke();
                break;
            case RPGUpgradesType.lifeSteal:
                lifeStealIndex++;
                OnLifeStealUpgraded?.Invoke();
                break;
            case RPGUpgradesType.movementSpeed:
                movementSpeedIndex++;
                OnMovementSpeedUpgraded?.Invoke();
                break;
            case RPGUpgradesType.powerupDur:
                powerupDurIndex++;
                OnPowerupDurUpgraded?.Invoke();
                break;
            case RPGUpgradesType.maxHealth:
                maxHealthIndex++;
                OnMaxHealthUpgraded?.Invoke();
                break;
            case RPGUpgradesType.dualWeapon:
                OnDualWeaponUpgraded?.Invoke();
                break;
            default:
                Debug.LogError("Upgrade Type is not defined");
                break;
        }

        saveManager.OnSaved?.Invoke();

        UpdateStats();
    }

    public void AttemptUpgradeAttackSpeed()
    {
        AttemptUpgradeStat(attackSpeedIndex, rpgSystemSO.GetAttackSpeedCosts(), CurrencyType.meat, RPGUpgradesType.attackSpeed);
    }
    public void AttemptUpgradeDamage()
    {
        AttemptUpgradeStat(damageIndex, rpgSystemSO.GetDamageCosts(), CurrencyType.meat, RPGUpgradesType.damage);
    }
    public void AttemptUpgradeRange()
    {
        AttemptUpgradeStat(rangeIndex, rpgSystemSO.GetRangeCosts(), CurrencyType.meat, RPGUpgradesType.range);
    }
    public void AttemptUpgradeMovementSpeed()
    {
        AttemptUpgradeStat(movementSpeedIndex, rpgSystemSO.GetMovementSpeedCosts(), CurrencyType.meat, RPGUpgradesType.movementSpeed);
    }
    public void AttemptUpgradeLifeSteal()
    {
        AttemptUpgradeStat(lifeStealIndex, rpgSystemSO.GetLifeStealCosts(), CurrencyType.meat, RPGUpgradesType.lifeSteal);
    }
    public void AttemptUpgradePowerupDuration()
    {
        AttemptUpgradeStat(powerupDurIndex, rpgSystemSO.GetPowerupDurCosts(), CurrencyType.meat, RPGUpgradesType.powerupDur);
    }
    public void AttemptUpgradeMaxHealth()
    {
        AttemptUpgradeStat(maxHealthIndex, rpgSystemSO.GetMaxHealthCosts(), CurrencyType.meat, RPGUpgradesType.maxHealth);
    }

    public void AttemptUpgradeDualWeapon()
    {

        if (dualWeaponIndex > 0) return;

        if (DecrementMeat(rpgSystemSO.GetDualWeaponCost()))
        {
            ActiveDualWeapon();
        }
        else
        {
            // pop up offer
        }
    }

    #endregion

    #region Dual Weapon
    private void ActiveDualWeapon()
    {
        dualWeaponIndex = 1;

        LookForDualWeapon();

        OnDualWeaponUpgraded?.Invoke();

        saveManager.OnSaved?.Invoke();
    }

    private void LookForDualWeapon()
    {
        if (dualWeaponIndex > 0)
        {
            OnWeaponActivision.Invoke(2);
        }
        else
        {
            OnWeaponActivision.Invoke(1);
        }
    }
    #endregion

    #region  MONEY

    public void IncrementMoney(int value)
    {
        money += value;
        MoneyChange();
    }

    public bool DecrementMoney(int value)
    {
        if (money >= value)
        {
            money -= value;
            MoneyChange();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoneyChange()
    {
        OnMoneyChange?.Invoke();
        saveManager.OnSaved?.Invoke();
    }
    #endregion

    #region  XP

    public void IncrementXP(int value)
    {
        experiencePoint += value;
        XPChange();
    }
    private void XPChange()
    {
        OnExperiencePointChange?.Invoke();
        saveManager.OnSaved?.Invoke();
    }

    private void ResetXP()
    {
        experiencePoint = 0;
        XPChange();
    }

    #endregion

    #region  Levelling
    public void LevelUpPlayer()
    {
        playerLevel++;
        ResetXP();
    }

    #endregion

    #region  Meat

    public void IncrementMeat(int value)
    {
        meat += value;
        MeatChange();
    }
    public bool DecrementMeat(int value)
    {
        if (meat >= value)
        {
            meat -= value;
            MeatChange();
            return true;
        }
        else
        {
            return false;
        }
    }
    private void MeatChange()
    {
        OnMeatChange?.Invoke();
        saveManager.OnSaved?.Invoke();
    }
    #endregion

    #region  Health Related

    public void FillCurrentHealth()
    {
        playerDeathHandler.SetCurrentHealth(maxHealth);
    }

    public void IncrementHealth(float lifeStolen)
    {
        float healValue = lifeStolen * lifeStealRate;
        if (healValue <= 0) return;

        if (playerDeathHandler.IncrementCurrentHealth(healValue))
        {
            OnLifeStolen?.Invoke();
        }
    }

    #endregion

    #region PowerUp

    private void IncrementPowerUp(float value)
    {
        if (isPowerupEnabled) return;

        OnPowerUpValueChanged?.Invoke(currentPowerUpValue / maxPowerUpFillValue);

        currentPowerUpValue += value;
        // event for ui
        if (currentPowerUpValue >= maxPowerUpFillValue)
        {
            EnablePowerUp();
        }
    }

    private void EnablePowerUp()
    {
        isPowerupEnabled = true;

        OnPowerUpEnabled?.Invoke();

        // değişkenler silinecek, indexler direkt updatestatsforpowerupa yollanacak
        int tempMovementIndex = Mathf.Clamp(movementSpeedIndex + powerUpUpgradeIndexValue, 0, rpgSystemSO.GetMovementSpeedValues().Count);
        int tempMaxHealthIndex = Mathf.Clamp(maxHealthIndex + powerUpUpgradeIndexValue, 0, rpgSystemSO.GetMaxHealthValues().Count);
        int tempAttackSpeedIndex = Mathf.Clamp(attackSpeedIndex + powerUpUpgradeIndexValue, 0, rpgSystemSO.GetAttackSpeedValues().Count);
        int tempDamageIndex = Mathf.Clamp(damageIndex + powerUpUpgradeIndexValue, 0, rpgSystemSO.GetDamageValues().Count);
        int tempRangeIndex = Mathf.Clamp(rangeIndex + powerUpUpgradeIndexValue, 0, rpgSystemSO.GetRangeValues().Count);

        UpdateStatsForPowerUp(tempMovementIndex, tempMaxHealthIndex, tempAttackSpeedIndex, tempDamageIndex, tempRangeIndex);
        StartCoroutine(DisablePowerUp());
    }

    IEnumerator DisablePowerUp()
    {
        yield return new WaitForSeconds(powerupDur);

        isPowerupEnabled = false;
        OnPowerUpDisabled?.Invoke();

        currentPowerUpValue = 0;
        // event for ui
        UpdateStats();
    }

    private void UpdateStatsForPowerUp(int newMovementSpeedIndex, int newMaxHealthIndex, int newAttackSpeedIndex, int newDamageIndex, int newRangeIndex)
    {
        // SO değişecek
        damage = rpgSystemSO.GetDamageValues()[newDamageIndex];
        attackSpeed = rpgSystemSO.GetAttackSpeedValues()[newAttackSpeedIndex];
        movementSpeed = rpgSystemSO.GetMovementSpeedValues()[newMovementSpeedIndex];
        maxHealth = rpgSystemSO.GetMaxHealthValues()[newMaxHealthIndex];
        range = rpgSystemSO.GetRangeValues()[newRangeIndex];
        OnRangeSet?.Invoke();
    }
    #endregion

    #region Earnings

    public void EarnBonusOnKill(float powerUpAddOnValue)
    {
        IncrementPowerUp(powerUpAddOnValue);
    }

    public void EarnBonusAtWaveEnd(int moneyValue, int xpValue, int meatValue)
    {
        IncrementMeat(meatValue);
        IncrementMoney(moneyValue);
        IncrementXP(xpValue);
    }

    #endregion

    #region Wave System

    public void WaveWon()
    {
        IncrementWaveIndex();
    }
    private void IncrementWaveIndex()
    {
        waveIndex++;
        OnWaveWon?.Invoke();
        saveManager.OnSaved?.Invoke();
    }

    #endregion

    #region CheckPoint Related

    public void SetWaveSystemBackToCheckpoint()
    {
        waveIndex = GameManager.instance.allCities[cityIndex].GetCurrentCheckpointIndex();
        saveManager.OnSaved?.Invoke();
    }

    public void CityChangerReached()
    {
        cityIndex++;
        saveManager.OnSaved?.Invoke();
    }

    #endregion

    #region Save & Load
    private void SavePlayerData()
    {
        SaveSystem.SavePlayerData(this);
    }

    private void LoadPlayerData()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData();

        if (playerData != null)
        {
            this.playerLevel = playerData.playerLevel;
            this.money = playerData.money;
            this.experiencePoint = playerData.experiencePoint;
            this.meat = playerData.meat;
            this.cityIndex = playerData.cityIndex;
            this.waveIndex = playerData.waveIndex;
            this.damageIndex = playerData.damageIndex;
            this.rangeIndex = playerData.rangeIndex;
            this.attackSpeedIndex = playerData.attackSpeedIndex;
            this.movementSpeedIndex = playerData.movementSpeedIndex;
            this.powerupDurIndex = playerData.powerupDurIndex;
            this.lifeStealIndex = playerData.lifeStealIndex;
            this.maxHealthIndex = playerData.maxHealthIndex;
            this.dualWeaponIndex = playerData.dualWeaponIndex;
        }

        UpdateStats();
    }

    private void ResetData()
    {
        SaveSystem.DeletePlayerData();
    }

    private void UpdateStats()
    {
        damage = rpgSystemSO.GetDamageValues()[damageIndex];
        attackSpeed = rpgSystemSO.GetAttackSpeedValues()[attackSpeedIndex];
        movementSpeed = rpgSystemSO.GetMovementSpeedValues()[movementSpeedIndex];
        powerupDur = rpgSystemSO.GetPowerupDurValues()[powerupDurIndex];
        lifeStealRate = rpgSystemSO.GetLifeStealValues()[lifeStealIndex];
        maxHealth = rpgSystemSO.GetMaxHealthValues()[maxHealthIndex];
        range = rpgSystemSO.GetRangeValues()[rangeIndex];

        OnRangeSet?.Invoke();
    }


    #endregion   

    #region  Getters & Setters
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    public float GetDamage()
    {
        return damage;
    }
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public int GetWaveIndex()
    {
        return waveIndex;
    }

    public int GetCityIndex()
    {
        return cityIndex;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public RPGSystemSO GetPlayerSO()
    {
        return rpgSystemSO;
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }
    public float GetRange()
    {
        return range;
    }

    public bool GetIsPowerupEnabled()
    {
        return isPowerupEnabled;
    }

    #endregion

}