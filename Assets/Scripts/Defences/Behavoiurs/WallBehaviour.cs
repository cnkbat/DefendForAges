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
        wallStats = GetComponent<WallStats>();
        cityManager = FindObjectOfType<CityManager>();
        cityManager.OnWaveCalled += ConnectToSpawner;
        wallStats.OnBuyDone += TargetRevived;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        cityManager.OnWaveCalled -= ConnectToSpawner;
        wallStats.OnBuyDone -= TargetRevived;
    }

    override protected void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        for (int i = 0; i < wallStats.healthParts.Count; i++)
        {
            if (currentHealth < wallStats.healthParts[i])
            {
                wallStats.wallParts[i].SetActive(false);
            }
        }
    }

    public void ConnectToSpawner()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.OnWaveCompleted += CheckForUpgradeable;
    }

    #region Repair Related

    public override void TargetRevived()
    {
        base.TargetRevived();

        foreach (var wallPart in wallStats.wallParts)
        {
            wallPart.SetActive(true);
        }


    }
    #endregion

}
