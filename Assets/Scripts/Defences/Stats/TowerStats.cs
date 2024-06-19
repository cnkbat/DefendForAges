using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : AttackerDefenceStat
{
    [Header("Tower SO")]
    [SerializeField] TowerSO towerSO;
    private float recovery;

    protected override void OnEnable()
    {
        base.OnEnable();
        attackerDefenceSO = towerSO;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    override protected void Start()
    {
        base.Start();
    }

    protected override void SetSOValues()
    {
        base.SetSOValues();
        recovery = towerSO.GetRecoveries()[upgradeIndex];
    }

    protected override void IncrementUpgradeIndex()
    {
        base.IncrementUpgradeIndex();
        SetSOValues();
    }

    #region Getters & Setters
    public float GetRecovery()
    {
        return recovery;
    }
    #endregion
}
