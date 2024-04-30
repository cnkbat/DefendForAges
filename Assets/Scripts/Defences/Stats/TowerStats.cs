using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : DefencesStatsBase
{
    [Header("Save & Load")]
    private float upgradeIndex;

    [Header("Ingame Values")]
    private float recovery;
    private float damage;
    private float attackSpeed;
}
