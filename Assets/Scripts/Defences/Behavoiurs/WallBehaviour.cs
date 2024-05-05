using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : DefencesBehaviourBase
{
    WallStats wallStats;
    CityManager cityManager;
    EnemySpawner enemySpawner;
    override protected void Start()
    {
        cityManager = FindObjectOfType<CityManager>();
        cityManager.OnWaveCalled += ConnectToSpawner;

        base.Start();

        defencesStatsBase = GetComponent<DefencesStatsBase>();
        ResetHealthValue();

        wallStats = GetComponent<WallStats>();
    }
    public override void TakeDamage(float dmg)
    {
        WallStats wallStats = GetComponent<WallStats>();
        currentHealth -= dmg;
        if (currentHealth < 12)
        {
            if (currentHealth < 10)
            {
                if (currentHealth < 8)
                {
                    if (currentHealth < 6)
                    {
                        if (currentHealth <= 0)
                        {
                            DestroyDefence();
                        }
                        wallStats.wallParts[3].SetActive(false);
                    }
                    wallStats.wallParts[2].SetActive(false);
                }
                wallStats.wallParts[1].SetActive(false);
            }
            wallStats.wallParts[0].SetActive(false);
        }


    }

    public void ConnectToSpawner()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnWaveCompleted += EnableRepair;
    }
    public void EnableRepair()
    {
        gameObject.SetActive(true);
        RepairDefences();
    }
    public void RepairDefences()
    {
        ResetHealthValue();
        foreach(var wallPart in wallStats.wallParts)
        {
            wallPart.SetActive(true);
        }

    }
    protected override void DestroyDefence()
    {
        isDestroyed = true;
        gameObject.SetActive(false);
        cityManager.UpdateTargetList();
        cityManager.OnTargetListUpdated?.Invoke();
    }
}
