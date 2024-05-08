using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Defence SO", menuName = "ScriptableObjects/Defence/Attack Defence SO")]

public class AttackerDefenceSO : StaticDefenceSO
{
    [Header("Attacking")]
    [SerializeField] private List<float> damageValues;
    [SerializeField] private List<float> fireRateValues;

    [Header("Costs")]
    [SerializeField] private List<int> upgradeCosts;


    public List<float> GetDamageValues() { return maxHealthValues; }
    public List<float> GetFireRateValues() { return maxHealthValues; }
    public List<float> GetUpgradeCosts() { return maxHealthValues; }

}
