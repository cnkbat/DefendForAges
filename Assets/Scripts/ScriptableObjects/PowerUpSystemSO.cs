using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "ScriptableObjects/PowerUp System")]

public class PowerUpSystemSO : ScriptableObject
{
    [Header("Attack Speed")]
    [SerializeField] private List<float> attackSpeedValues;

    [Header("Range")]
    [SerializeField] private List<float> rangeValues;

    [Header("Movement Speed")]
    [SerializeField] private List<float> movementSpeedValues;

    #region  Getters

    public List<float> GetAttackSpeedValues() { return attackSpeedValues; }
    public List<float> GetRangeValues() { return rangeValues; }
    public List<float> GetMovementSpeedValues() { return movementSpeedValues; }



    #endregion


}
