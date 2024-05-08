using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Static Defence SO", menuName = "ScriptableObjects/Defence/ Static Defence SO")]

public class StaticDefenceSO : ScriptableObject
{
    [SerializeField] protected List<float> maxHealthValues;

    public List<float> GetMaxHealthValues() { return maxHealthValues; }
}
