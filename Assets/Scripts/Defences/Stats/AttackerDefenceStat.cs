using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackerDefenceStat : DefencesStatsBase
{
    PlayerStats playerStats;

    [Header("Attacker SO")]
    [SerializeField] protected AttackerDefenceSO attackerDefenceSO;

    [Header("Ingame Values")]
    protected float damage;
    protected float attackSpeed;

    [Header("Ingame Values")]
    [SerializeField] List<Weapon> weapons;
    private List<int> upgradeEnablingIndexes;

    [Header("Events")]
    public Action OnUpgraded;

    protected override void OnEnable()
    {
        base.OnEnable();

        playerStats = PlayerStats.instance;
        playerStats.OnWaveWon += HandleLoableState;

        defenceSO = attackerDefenceSO;
        OnUpgraded += HandleLoableState;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playerStats.OnWaveWon -= HandleLoableState;
        OnUpgraded -= HandleLoableState;
    }

    protected override void Start()
    {
        base.Start();
        HandleLoableState();
    }

    #region Buying

    public override void BuyDone()
    {
        base.BuyDone();

        IncrementUpgradeIndex();
        OnUpgraded?.Invoke();
        SetSOValues();
    }

    public void HandleLoableState()
    {
        // if isRepair sonrasında buraya return sonra bi daha check
        // tabi para da 0 lanacak 

        Debug.Log(upgradeEnablingIndexes[upgradeIndex]);

        if (playerStats.GetPlayerLevel() >= upgradeEnablingIndexes[upgradeIndex])
        {
            loadableBase.gameObject.SetActive(true);
        }
        else
        {
            loadableBase.gameObject.SetActive(false);
        }

    }

    #endregion

    #region Data Transfer

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
        upgradeEnablingIndexes = attackerDefenceSO.GetUpgradeEnablingIndexes();

        loadableBase.SetCurrentCostLeftForUpgrade(attackerDefenceSO.GetUpgradeCosts()[upgradeIndex]);
    }

    protected override void LoadDefenceData()
    {
        base.LoadDefenceData();

        DefencesData defencesData = SaveSystem.LoadDefenceData(defenceID);

        if (defencesData == null)
        {
            loadableBase.SetCurrentCostLeftForUpgrade(attackerDefenceSO.GetUpgradeCosts()[upgradeIndex]);
        }
        else
        {
            loadableBase.SetCurrentCostLeftForUpgrade(defencesData.currentCostLeftForUpgrade);
        }
    }

    #endregion

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
