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

    protected override void OnEnable()
    {
        base.OnEnable();
        defenceSO = attackerDefenceSO;
        loadableBase.SetCost(attackerDefenceSO.GetUpgradeCosts());
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void SetSOValues()
    {
        base.SetSOValues();
        damage = attackerDefenceSO.GetDamageValues()[upgradeIndex];
        attackSpeed = attackerDefenceSO.GetAttackSpeedValues()[upgradeIndex];
    }


    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetDamage()
    {
        return damage;
    }
}
