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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DestroyDefence();
        }
    }
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        if (currentHealth < wallStats.healthParts[0])
        {
            if (currentHealth < wallStats.healthParts[1])
            {
                if (currentHealth < wallStats.healthParts[2])
                {
                    if (currentHealth < wallStats.healthParts[3])
                    {
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
