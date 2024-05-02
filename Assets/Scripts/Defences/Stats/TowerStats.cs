using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : DefencesStatsBase
{

    [Header("Save & Load")]
    private float upgradeIndex;

    [Header("Ingame Values")]
    [SerializeField] List<GameObject> weapons;
    private float recovery;
    private float damage;
    private float attackSpeed;
    private int weaponIndex;

    private void Start() 
    {
        
    }

    private void OnUpgradeCompleted()
    {
        
    }
}
