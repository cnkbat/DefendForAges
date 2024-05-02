using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : Singleton<PlayerStats>
{
    SaveManager saveManager;

    [SerializeField] private RPGSystemSO rPGSystemSO;

    [Header("Saved Indexes")]
    public int money;
    public int experiencePoint;
    public int damageIndex;
    public int attackSpeedIndex;
    public int movementSpeedIndex;
    public int powerupDurIndex;
    public int lifeStealIndex;
    public int maxHealthIndex;
    public bool isDualWeaponActiveSavedValue;
    public int waveIndex;
    public int cityIndex;

    [Header("Ingame Values")]
    private int currentCityIndex;
    private int currentWaveIndex;
    [SerializeField] private int currentXP;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float powerupDur;
    [SerializeField] private float lifeSteal;
    [SerializeField] private bool isDualWeaponActive;
    [SerializeField] private float maxHealth;
    private float currentHealth;
    DeathHandler deathHandler;

    [Header("Events")]
    public Action<int, int, float> OnKillEnemy;
    public Action OnDataChanged;
    public Action OnWaveWon;
    public Action OnMovementChanged;
    public Action OnRevive;

    private void Start()
    {
        saveManager = SaveManager.instance;
        deathHandler = GetComponent<DeathHandler>();

        if (saveManager != null)
        {
            saveManager.OnSaved += SavePlayerData;
            OnDataChanged += saveManager.DataChanged;
        }

        OnKillEnemy += EarnBonusOnKill;

        OnRevive += FillCurrentHealth;

        LoadPlayerData();
        FillCurrentHealth();

    }

    #region  MONEY
    public void IncrementMoney(int value)
    {
        money += value;
    }

    public void DecrementMoney(int value)
    {
        money -= value;
    }
    #endregion

    #region  XP
    public void IncrementExp(int value)
    {
        currentXP += value;
    }
    public void DecrementXP(int value)
    {
        currentXP -= value;
    }
    #endregion

    #region Wave System

    public void EarnBonusOnKill(int moneyValue, int xpValue, float powerUpAddOnValue)
    {
        IncrementMoney(moneyValue);
        IncrementExp(xpValue);
        // power up eklenmedi daha
    }

    public void IncrementWaveIndex()
    {
        currentWaveIndex++;

        OnDataChanged?.Invoke();

        OnWaveWon?.Invoke();
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

        currentCityIndex++;
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
            this.isDualWeaponActiveSavedValue = playerData.isDualWeaponActiveSavedValue;
        }

        UpdateStats();
    }

    private void UpdateStats()
    {
        damage = rPGSystemSO.damageValues[damageIndex];
        attackSpeed = rPGSystemSO.attackSpeedValues[attackSpeedIndex];
        movementSpeed = rPGSystemSO.movementSpeedValues[movementSpeedIndex];
        powerupDur = rPGSystemSO.powerupDurValues[powerupDurIndex];
        lifeSteal = rPGSystemSO.lifeStealValues[lifeStealIndex];
        maxHealth = rPGSystemSO.maxHealthValues[maxHealthIndex];
        currentXP = this.experiencePoint;
        currentCityIndex = this.cityIndex;
        currentWaveIndex = this.waveIndex;
    }


    #endregion 

    public void FillCurrentHealth()
    {
        deathHandler.SetCurrentHealth(maxHealth);
    }

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

    public int GetCurrentWaveIndex()
    {
        return currentWaveIndex;
    }

    public int GetCurrentCityIndex()
    {
        return currentCityIndex;
    }

    #endregion
}
