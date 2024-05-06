using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/RPG System")]

public class RPGSystemSO : ScriptableObject
{

    [Header("Attack Speed")]

    [SerializeField] private List<float> attackSpeedValues;
    [SerializeField] private List<int> attackSpeedCosts;

    [Header("Damage")]
    [SerializeField] private List<float> damageValues;
    [SerializeField] private List<int> damageCosts;

    [Header("Movement Speed")]
    [SerializeField] private List<float> movementSpeedValues;
    [SerializeField] private List<int> movementSpeedCosts;

    [Header("Life Steal")]
    [SerializeField] private List<float> lifeStealValues;
    [SerializeField] private List<int> lifeStealCosts;

    [Header("Power Up")]
    [SerializeField] private List<float> powerupDurValues;
    [SerializeField] private List<int> powerupDurCosts;

    [Header("Max Health")]
    [SerializeField] private List<float> maxHealthValues;
    [SerializeField] private List<int> maxHealthCosts;

    [Header("Dual Weapon Upgrade")]
    [SerializeField] private int dualWeaponCost;

    #region  Getters

    public List<float> GetAttackSpeedValues() { return attackSpeedValues; }
    public List<int> GetAttackSpeedCosts() { return attackSpeedCosts; }
    public List<float> GetDamageValues() { return damageValues; }
    public List<int> GetDamageCosts() { return damageCosts; }
    public List<float> GetMovementSpeedValues() { return movementSpeedValues; }
    public List<int> GetMovementSpeedCosts() { return movementSpeedCosts; }
    public List<float> GetLifeStealValues() { return lifeStealValues; }
    public List<int> GetLifeStealCosts() { return lifeStealCosts; }
    public List<float> GetPowerupDurValues() { return powerupDurValues; }
    public List<int> GetPowerupDurCosts() { return powerupDurCosts; }
    public List<float> GetMaxHealthValues() { return maxHealthValues; }
    public List<int> GetMaxHealthCosts() { return maxHealthCosts; }
    public int GetDualWeaponCost() { return dualWeaponCost; }

    #endregion


}
