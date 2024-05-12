using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : AttackerDefenceStat
{
    [Header("Tower SO")]
    [SerializeField] TowerSO towerSO;

    [Header("Ingame Values")]
    [SerializeField] List<Weapon> weapons;
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

        for (int i = 0; i < weapons.Count; i++)
        {
            if (towerSO.GetWeaponSpawnIndexes()[i] <= upgradeIndex)
            {
                weapons[i].gameObject.SetActive(true);
            }
        }
    }


    #region Getters & Setters
    public float GetRecovery()
    {
        return recovery;
    }

    public List<Weapon> GetWeapons()
    {
        return weapons;
    }
    #endregion
}
