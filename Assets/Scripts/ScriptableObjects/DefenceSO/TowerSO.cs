using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower SO", menuName = "ScriptableObjects/Defence/Tower SO")]

public class TowerSO : AttackerDefenceSO
{
    [Header("For Tower")]
    [SerializeField] private List<float> recoveries;
    [SerializeField] private List<int> weaponSpawnIndexes;

    public List<float> GetRecoveries()
    {
        return recoveries;
    }

    public List<int> GetWeaponSpawnIndexes() {return weaponSpawnIndexes;}

}
