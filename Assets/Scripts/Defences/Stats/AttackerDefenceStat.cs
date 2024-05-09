using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackerDefenceStat : DefencesStatsBase
{
    [Tooltip("Attak defansifler için sadece burası doldurulmalı.")]
    [SerializeField] AttackerDefenceSO attackerDefenceSO;

    [Header("Ingame Values")]
    private float damage;
    private float attackSpeed;

    [Header("Events")]
    public Action OnUpgraded;

    protected override void OnEnable()
    {
        base.OnEnable();
        defenceSO = attackerDefenceSO;

        loadableBase.SetCost(attackerDefenceSO.GetUpgradeCosts());
        loadableBase.SetCostIndex(upgradeIndex);
        
        OnUpgraded += SetSOValues;
        OnUpgraded += loadableBase.UpdateCurrentCostLeft;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        OnUpgraded -= SetSOValues;
        OnUpgraded -= loadableBase.UpdateCurrentCostLeft;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void BuyDone()
    {
        base.BuyDone();
        IncrementUpgradeIndex();
        Debug.Log("wtf");
        loadableBase.SetCostIndex(upgradeIndex);
        OnUpgraded?.Invoke();
    }

    protected override void SetSOValues()
    {
        base.SetSOValues();
        damage = attackerDefenceSO.GetDamageValues()[upgradeIndex];
        attackSpeed = attackerDefenceSO.GetAttackSpeedValues()[upgradeIndex];
    }


    #region Getters & Setters
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetDamage()
    {
        return damage;
    }
    #endregion

}
