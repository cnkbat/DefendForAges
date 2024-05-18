using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : DefencesBehaviourBase
{

    WallStats wallStats;
    NavMeshManager navMeshManager;
    [SerializeField] private List<GameObject> nearbyEnvironment;

    protected override void OnEnable()
    {
        base.OnEnable();
        wallStats = GetComponent<WallStats>();

        navMeshManager = NavMeshManager.instance;

        wallStats.OnBuyDone += TargetRevived;

        playerStats.OnWaveWon += CheckForUpgradeable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        wallStats.OnBuyDone -= TargetRevived;

        playerStats.OnWaveWon -= CheckForUpgradeable;
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

    #region Repair Related

    public override void TargetRevived()
    {
        base.TargetRevived();

        foreach (var wallPart in wallStats.wallParts)
        {
            wallPart.SetActive(true);
        }

        for (int i = 0; i < nearbyEnvironment.Count; i++)
        {
            nearbyEnvironment[i].SetActive(true);
        }

        navMeshManager.BakeNavMesh();
    }

    protected override void DestroyDefence()
    {
        base.DestroyDefence();

        for (int i = 0; i < nearbyEnvironment.Count; i++)
        {
            nearbyEnvironment[i].SetActive(false);
        }

        navMeshManager.BakeNavMesh();
    }
    #endregion

}
