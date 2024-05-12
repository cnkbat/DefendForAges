using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityManagerData 
{
    public int buyedAreaIndex;
    public int towerUpgradeIndex;
    public CityManagerData(CityManager cityManager)
    {
        this.buyedAreaIndex = cityManager.buyedAreaIndex;
        this.towerUpgradeIndex = cityManager.towerUpgradeIndex;
    }
}
