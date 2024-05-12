using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Defence SO", menuName = "ScriptableObjects/Defence/Attack Defence SO")]

public class AttackerDefenceSO : StaticDefenceSO
{
    [Header("Attacking")]
    [SerializeField] private List<float> damageValues;
    [SerializeField] private List<float> attackSpeedValues;

    [Header("Costs")]
    [SerializeField] private List<int> upgradeCosts;


    public List<float> GetDamageValues() { return damageValues; }
    public List<float> GetAttackSpeedValues() { return attackSpeedValues; }
    public List<int> GetUpgradeCosts() { return upgradeCosts; }

}
