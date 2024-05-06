using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentEraIndex;
    public int currentTimelineIndex;
    public int money;
    public int experiencePoint;
    public int cityIndex;
    public int waveIndex;

    public int damageIndex;
    public int attackSpeedIndex;
    public int movementSpeedIndex;
    public int powerupDurIndex;
    public int lifeStealIndex;
    public int maxHealthIndex;
    public bool isDualWeaponActiveSavedValue;

    //    public bool isHapticActive, isSoundFXActive;

    public PlayerData(PlayerStats playerStats)
    {

        this.money = playerStats.money;
        this.experiencePoint = playerStats.experiencePoint;
        this.cityIndex = playerStats.cityIndex;
        this.waveIndex = playerStats.waveIndex;
        this.damageIndex = playerStats.damageIndex;
        this.attackSpeedIndex = playerStats.attackSpeedIndex;
        this.movementSpeedIndex = playerStats.movementSpeedIndex;
        this.powerupDurIndex = playerStats.powerupDurIndex;
        this.lifeStealIndex = playerStats.lifeStealIndex;
        this.maxHealthIndex = playerStats.maxHealthIndex;
        this.isDualWeaponActiveSavedValue = playerStats.isDualWeaponActiveSavedValue;

        // isHapticActive = playerStats.isHapticActive;
        // isSoundFXActive = playerStats.isSoundFXActive;
    }
}
