using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Defence SO", menuName = "ScriptableObjects/Defence/Attack Defence SO" )]

public class AttackerDefenceSO : ScriptableObject
{
    [SerializeField] public List<float> maxHealthValues;
    [SerializeField] public List<float> damageValues;
    [SerializeField] public List<float> fireRateValues;
    [SerializeField] public List <int> upgradeCosts;
}
