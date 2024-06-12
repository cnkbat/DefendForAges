using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "ScriptableObjects/PowerUp System")]

public class PowerUpSystemSO : ScriptableObject
{
    [Header("Attack Speed")]
    [SerializeField] private List<float> attackSpeedValues;
    [SerializeField] private List<int> attackSpeedCosts;

    [Header("Range")]
    [SerializeField] private List<float> rangeValues;
    [SerializeField] private List<int> rangeCosts;

    [Header("Movement Speed")]
    [SerializeField] private List<float> movementSpeedValues;
    [SerializeField] private List<int> movementSpeedCosts;


    #region  Getters

    public List<float> GetAttackSpeedValues() { return attackSpeedValues; }
    public List<int> GetAttackSpeedCosts() { return attackSpeedCosts; }
    public List<float> GetRangeValues() { return rangeValues; }
    public List<int> GetRangeCosts() { return rangeCosts; }
    public List<float> GetMovementSpeedValues() { return movementSpeedValues; }
    public List<int> GetMovementSpeedCosts() { return movementSpeedCosts; }



    #endregion


}
