using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : DefencesBehaviourBase
{
    WallStats wallStats;
    CityManager cityManager;
    EnemySpawner enemySpawner;

    protected override void OnEnable()
    {
        base.OnEnable();
        cityManager = FindObjectOfType<CityManager>();
        cityManager.OnWaveCalled += ConnectToSpawner;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        cityManager.OnWaveCalled -= ConnectToSpawner;
    }

    override protected void Start()
    {
        base.Start();

        defencesStatsBase = GetComponent<DefencesStatsBase>();
        wallStats = GetComponent<WallStats>();
    }

    public override void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth < wallStats.healthParts[3])
        {
            if (currentHealth < wallStats.healthParts[2])
            {
                if (currentHealth < wallStats.healthParts[1])
                {
                    if (currentHealth < wallStats.healthParts[0])
                    {
                        if (currentHealth <= 0)
                        {
                            DestroyDefence();
                        }
                        wallStats.wallParts[0].SetActive(false);
                    }
                    wallStats.wallParts[1].SetActive(false);
                }
                wallStats.wallParts[2].SetActive(false);
            }
            wallStats.wallParts[3].SetActive(false);
        }
    }

    public void ConnectToSpawner()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnWaveCompleted += EnableRepair;
    }

    #region Repair Related
    public void EnableRepair()
    {
        gameObject.SetActive(true);
        RepairDefences();
    }
    public void RepairDefences()
    {
        ResetHealthValue();
        foreach (var wallPart in wallStats.wallParts)
        {
            wallPart.SetActive(true);
        }

    }
    #endregion 

    protected override void DestroyDefence()
    {
        gameObject.SetActive(false);
        base.DestroyDefence();
    }
}
