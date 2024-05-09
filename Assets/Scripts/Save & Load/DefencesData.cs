[System.Serializable]
public class DefencesData
{
    public int upgradeIndex;
    public int currentCostLeftForUpgrade;

    public DefencesData(DefencesStatsBase defencesStatsBase)
    {
        this.upgradeIndex = defencesStatsBase.upgradeIndex;
        currentCostLeftForUpgrade = defencesStatsBase.loadableBase.currentCostLeftForUpgrade;
    }
}