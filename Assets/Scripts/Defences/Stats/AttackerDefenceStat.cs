using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackerDefenceStat : DefencesStatsBase
{

    [Tooltip("Attack defansifler için sadece burası doldurulmalı.")]
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
        OnUpgraded += SetSOValues;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        OnUpgraded -= SetSOValues;
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void BuyDone()
    {
        base.BuyDone();
        IncrementUpgradeIndex();
        OnUpgraded?.Invoke();
    }

    protected override void SetSOValues()
    {
        base.SetSOValues();
        damage = attackerDefenceSO.GetDamageValues()[upgradeIndex];
        attackSpeed = attackerDefenceSO.GetAttackSpeedValues()[upgradeIndex];
    }

    protected override void LoadDefenceData()
    {
        base.LoadDefenceData();

        DefencesData defencesData = SaveSystem.LoadDefenceData(defenceID);

        if (defencesData == null)
        {
            loadableBase.SetCurrentCostLeftForUpgrade(attackerDefenceSO.GetUpgradeCosts()[upgradeIndex]);
        }

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
