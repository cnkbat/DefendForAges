using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuyableAreaData
{
    public int currentCostLeftForUpgrade;

    public BuyableAreaData(BuyableArea buyableArea)
    {
        this.currentCostLeftForUpgrade = buyableArea.loadableBase.currentCostLeftForUpgrade;
    }
}