using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentEraIndex;
    public int currentTimelineIndex;
    public int money;
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

        money = playerStats.money;

        // isHapticActive = playerStats.isHapticActive;
        // isSoundFXActive = playerStats.isSoundFXActive;
    }
}
