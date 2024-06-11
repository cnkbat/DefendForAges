[System.Serializable]
public class PlayerData
{

    public int playerLevel;
    public int money;
    public int experiencePoint;
    public int meat;

    public int cityIndex;
    public int waveIndex;

    public int attackSpeedIndex;
    public int damageIndex;
    public int rangeIndex;
    public int movementSpeedIndex;
    public int powerupDurIndex;
    public int lifeStealIndex;
    public int maxHealthIndex;
    public int dualWeaponIndex;

    //    public bool isHapticActive, isSoundFXActive;

    public PlayerData(PlayerStats playerStats)
    {
        this.playerLevel = playerStats.playerLevel;

        this.money = playerStats.money;
        this.experiencePoint = playerStats.experiencePoint;
        this.meat = playerStats.meat;

        this.cityIndex = playerStats.cityIndex;
        this.waveIndex = playerStats.waveIndex;

        this.damageIndex = playerStats.damageIndex;
        this.attackSpeedIndex = playerStats.attackSpeedIndex;
        this.rangeIndex = playerStats.rangeIndex;

        this.movementSpeedIndex = playerStats.movementSpeedIndex;
        this.powerupDurIndex = playerStats.powerupDurIndex;

        this.lifeStealIndex = playerStats.lifeStealIndex;
        this.maxHealthIndex = playerStats.maxHealthIndex;

        this.dualWeaponIndex = playerStats.dualWeaponIndex;

        // isHapticActive = playerStats.isHapticActive;
        // isSoundFXActive = playerStats.isSoundFXActive;
    }
}
