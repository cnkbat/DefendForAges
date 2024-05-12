using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackerDefenceStat : DefencesStatsBase
{

    [Header("Attacker SO")]
    [SerializeField] protected AttackerDefenceSO attackerDefenceSO;

    [Header("Ingame Values")]
    protected float damage;
    protected float attackSpeed;

    [Header("Ingame Values")]
    [SerializeField] List<Weapon> weapons;

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

        for (int i = 0; i < weapons.Count; i++)
        {
            if (attackerDefenceSO.GetWeaponSpawnIndexes()[i] <= upgradeIndex)
            {
                weapons[i].gameObject.SetActive(true);
            }
        }

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

    public List<Weapon> GetWeapons()
    {
        return weapons;
    }
    #endregion

}
