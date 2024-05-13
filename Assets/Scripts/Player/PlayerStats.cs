using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerStats : Singleton<PlayerStats>
{
    SaveManager saveManager;
    LevelSystem levelSystem;

    [SerializeField] private RPGSystemSO rpgSystemSO;

    [Header("Saved Indexes")]
    public int playerLevel;
    public int money;
    public int experiencePoint;
    public int meat;
    public int damageIndex;
    public int attackSpeedIndex;
    public int movementSpeedIndex;
    public int powerupDurIndex;
    public int lifeStealIndex;
    public int maxHealthIndex;
    public bool isDualWeaponActive;
    public int waveIndex;
    public int cityIndex;

    [Header("Ingame Values")]
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float powerupDur;
    [SerializeField] private float lifeSteal;
    [SerializeField] private float maxHealth;
    DeathHandler deathHandler;

    [Header("Power Up")]
    [SerializeField] private float maxPowerUpFillValue;
    [SerializeField] private int powerUpUpgradeIndexValue;
    private bool isPowerupEnabled;
    private float currentPowerUpValue;

    [Header("-------Stat Change Events ------")]
    public Action<int, int, int, float> OnKillEnemy;

    [Header("Upgrade Events")]
    public Action OnAttackSpeedUpgraded;
    public Action OnDamageUpgraded;
    public Action OnLifeStealUpgraded;
    public Action OnMovementSpeedUpgraded;
    public Action OnPowerupDurUpgraded;
    public Action OnMaxHealthUpgraded;
    public Action OnDualWeaponUpgraded;

    [Header("Action Events")]
    public Action OnRevive;
    public Action OnWaveWon;
    public Action<int> OnWeaponActivision;

    [Header("UI Events")]
    public Action OnExperiencePointChange;
    public Action OnMoneyChange;
    public Action OnMeatChange;

    protected override void Awake()
    {
        LoadPlayerData();
        levelSystem = GetComponent<LevelSystem>();
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
        OnRevive += FillCurrentHealth;

        levelSystem.OnLevelUp += ResetXP;
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
        OnRevive -= FillCurrentHealth;
    }

    #endregion

    private void Start()
    {
        saveManager = SaveManager.instance;
        deathHandler = GetComponent<DeathHandler>();

        FillCurrentHealth();
        LookForDualWeapon();
    }

    #region Upgrading

    private void AttemptUpgradeStat(int indexToUpgrade, List<int> cost, CurrencyType currencyType, RPGUpgradesType upgradeType = RPGUpgradesType.empty)
    {
        if (currencyType == CurrencyType.money)
        {
            if (cost[indexToUpgrade] <= money)
            {
                UpgradeSuccesful(upgradeType);
                DecrementMoney(cost[indexToUpgrade]);
            }
            else
            {
                // pop up offer
            }
        }
        else if (currencyType == CurrencyType.meat)
        {
            if (cost[indexToUpgrade] <= meat)
            {
                UpgradeSuccesful(upgradeType);
                DecrementMeat(cost[indexToUpgrade]);
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

        if (upgradesType == RPGUpgradesType.attackSpeed)
        {
            attackSpeedIndex++;
            OnAttackSpeedUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.damage)
        {
            damageIndex++;
            OnDamageUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.lifeSteal)
        {
            lifeStealIndex++;
            OnLifeStealUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.movementSpeed)
        {
            movementSpeedIndex++;
            OnMovementSpeedUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.powerupDur)
        {
            powerupDurIndex++;
            OnPowerupDurUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.maxHealth)
        {
            maxHealthIndex++;
            OnMaxHealthUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.dualWeapon)
        {
            OnDualWeaponUpgraded?.Invoke();
        }

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

        if (isDualWeaponActive) return;

        if (rpgSystemSO.GetDualWeaponCost() <= meat)
        {
            ActiveDualWeapon();
            DecrementMeat(rpgSystemSO.GetDualWeaponCost());
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
        isDualWeaponActive = true;
        LookForDualWeapon();

        OnDualWeaponUpgraded?.Invoke();
    }

    private void LookForDualWeapon()
    {
        if (isDualWeaponActive)
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

    public void DecrementMoney(int value)
    {
        money -= value;
        MoneyChange();
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

    #region  Meat

    public void IncrementMeat(int value)
    {
        meat += value;
        MeatChange();
    }
    public void DecrementMeat(int value)
    {
        meat -= value;
        MeatChange();
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
        deathHandler.SetCurrentHealth(maxHealth);
    }

    public void IncrementHealth(float lifeStolen)
    {
        deathHandler.IncerementCurrentHealth(lifeStolen);
    }

    #endregion

    #region PowerUp

    private void IncrementPowerUp(float value)
    {
        if (isPowerupEnabled) return;

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

        int tempMovementIndex = movementSpeedIndex + powerUpUpgradeIndexValue;
        int tempMaxHealthIndex = maxHealthIndex + powerUpUpgradeIndexValue;
        int tempAttackSpeedIndex = attackSpeedIndex + powerUpUpgradeIndexValue;
        int tempDamageIndex = damageIndex + powerUpUpgradeIndexValue;

        UpdateStatsForPowerUp(tempMovementIndex, tempMaxHealthIndex, tempAttackSpeedIndex, tempDamageIndex);
        StartCoroutine(DisablePowerUp());
    }

    IEnumerator DisablePowerUp()
    {
        yield return new WaitForSeconds(powerupDur);

        currentPowerUpValue = 0;
        // event for ui
        UpdateStats();
    }

    private void UpdateStatsForPowerUp(int movementSpeedIndex, int maxHealthIndex, int attackSpeedIndex, int damageIndex)
    {
        damage = rpgSystemSO.GetDamageValues()[damageIndex];
        attackSpeed = rpgSystemSO.GetAttackSpeedValues()[attackSpeedIndex];
        movementSpeed = rpgSystemSO.GetMovementSpeedValues()[movementSpeedIndex];
        maxHealth = rpgSystemSO.GetMaxHealthValues()[maxHealthIndex];
    }
    #endregion

    #region Wave System

    public void WaveWon()
    {
        IncrementWaveIndex();
    }

    public void EarnBonusOnKill(int moneyValue, int xpValue, int meatValue, float powerUpAddOnValue)
    {
        IncrementMoney(moneyValue);
        IncrementXP(xpValue);
        IncrementMeat(meatValue);
        IncrementPowerUp(powerUpAddOnValue);
    }

    private void IncrementWaveIndex()
    {
        waveIndex++;
        Debug.Log("Wave Index = " + waveIndex);
        OnWaveWon?.Invoke();
        saveManager.OnSaved?.Invoke();
    }

    #endregion

    #region CheckPoint Related
    public void SetWaveSystemBackToCheckpoint()
    {
        // kaybettiğimizde devreye girecek
        // gamemanagerdan eventle ulaşılması
        // wave indexin en son checkpoint değerine atanması
        // save edilmesi
    }

    public void CheckPointReached()
    {

        cityIndex++;
        // coroutinele bağlanabilir
        // diğer şehre hareketi
        // kazanılan bonus
        // araya giren ads popupları   
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
            this.attackSpeedIndex = playerData.attackSpeedIndex;
            this.movementSpeedIndex = playerData.movementSpeedIndex;
            this.powerupDurIndex = playerData.powerupDurIndex;
            this.lifeStealIndex = playerData.lifeStealIndex;
            this.maxHealthIndex = playerData.maxHealthIndex;
            this.isDualWeaponActive = playerData.isDualWeaponActiveSavedValue;
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
        lifeSteal = rpgSystemSO.GetLifeStealValues()[lifeStealIndex];
        maxHealth = rpgSystemSO.GetMaxHealthValues()[maxHealthIndex];
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

    #endregion

}