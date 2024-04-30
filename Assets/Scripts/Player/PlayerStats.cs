using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    [SerializeField] private RPGSystemSO rPGSystemSO;

    [Header("Saved Indexes")]
    public int money;
    public int damageIndex;
    public int attackSpeedIndex;
    public int movementSpeedIndex;
    public int powerupDurIndex;
    public int lifeStealIndex;
    public int maxHealthIndex;
    public bool isDualWeaponActiveSavedValue;

    [Header("Ingame Values")]
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float powerupDur;
    [SerializeField] private float lifeSteal;
    [SerializeField] private bool isDualWeaponActive;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    private void Start()
    {
        LoadPlayerData();
        FillCurrentHealth();
    }

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
    }

    #endregion 

    private void FillCurrentHealth()
    {
        currentHealth = maxHealth;
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

    #endregion
}
