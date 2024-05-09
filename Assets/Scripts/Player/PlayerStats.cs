using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : Singleton<PlayerStats>
{
    SaveManager saveManager;

    [SerializeField] private RPGSystemSO rpgSystemSO;

    [Header("Saved Indexes")]
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


    [Header("Save Load Events")]
    public Action OnDataChanged;

    [Header("Action Events")]
    public Action OnRevive;
    public Action OnWaveWon;

    [Header("UI Events")]
    public Action OnExperiencePointChange;
    public Action OnMoneyChange;
    public Action OnMeatChange;

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
            OnDataChanged += saveManager.DataChanged;
            saveManager.OnResetData += ResetData;
        }

        OnKillEnemy += EarnBonusOnKill;
        OnRevive += FillCurrentHealth;
    }

    private void OnDisable()
    {
        saveManager = SaveManager.instance;

        if (saveManager != null)
        {
            saveManager.OnSaved -= SavePlayerData;
            OnDataChanged -= saveManager.DataChanged;
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
                UpgradeSuccesful(indexToUpgrade, upgradeType);
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
                UpgradeSuccesful(indexToUpgrade, upgradeType);
                DecrementMeat(cost[indexToUpgrade]);
            }
            else
            {
                // pop up offer
            }
        }

    }

    private void UpgradeSuccesful(int indexToUpgrade, RPGUpgradesType upgradesType)
    {

        indexToUpgrade++;
        UpdateStats();
        Debug.Log("upgrade succes");


        if (upgradesType == RPGUpgradesType.empty) return;

        if (upgradesType == RPGUpgradesType.attackSpeed)
        {
            OnAttackSpeedUpgraded?.Invoke();
            Debug.Log("attack speed invoke");
        }
        else if (upgradesType == RPGUpgradesType.damage)
        {
            OnDamageUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.lifeSteal)
        {
            OnLifeStealUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.movementSpeed)
        {
            OnMovementSpeedUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.powerupDur)
        {
            OnPowerupDurUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.maxHealth)
        {
            OnMaxHealthUpgraded?.Invoke();
        }
        else if (upgradesType == RPGUpgradesType.dualWeapon)
        {
            OnDualWeaponUpgraded?.Invoke();
        }
    }

    public void AttemptUpgradeAttackSpeed()
    {
        Debug.Log("button click");
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

        if (rpgSystemSO.GetDualWeaponCost() >= meat)
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

        OnDualWeaponUpgraded?.Invoke();
    }

    private void LookForDualWeapon()
    {
        if (isDualWeaponActive)
        {
            // ikinci silahın aktivasyonu
            // animasyonun dual weapon yapısına geçmesi
            Debug.Log("dual weapon active");
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
        // power up eklenmedi daha
    }

    private void IncrementWaveIndex()
    {
        waveIndex++;
        OnWaveWon?.Invoke();
        OnDataChanged?.Invoke();
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
        OnDataChanged?.Invoke();

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
            this.money = playerData.money;
            this.experiencePoint = playerData.experiencePoint;
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
        PlayerData playerData = SaveSystem.LoadPlayerData();

        if (playerData != null)
        {
            this.money = 0;
            this.experiencePoint = 0;
            this.meat = 0;
            this.cityIndex = 0;
            this.waveIndex = 0;
            this.damageIndex = 0;
            this.attackSpeedIndex = 0;
            this.movementSpeedIndex = 0;
            this.powerupDurIndex = 0;
            this.lifeStealIndex = 0;
            this.maxHealthIndex = 0;
            this.isDualWeaponActive = false;
        }

        SavePlayerData();
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

    #region  Health
    public void FillCurrentHealth()
    {
        deathHandler.SetCurrentHealth(maxHealth);
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

    public RPGSystemSO GetPlayerSO()
    {
        return rpgSystemSO;
    }

    #endregion

}
