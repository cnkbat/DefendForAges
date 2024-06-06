using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : DefencesBehaviourBase
{

    WallStats wallStats;

    [SerializeField] private List<Animator> nearbyEnvironment;

    protected override void OnEnable()
    {
        base.OnEnable();
        wallStats = GetComponent<WallStats>();


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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        { DestroyDefence(); }
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

        for (int i = 0; i < wallStats.wallHolderParts.Count; i++)
        {
            wallStats.wallHolderParts[i].SetActive(true);
        }

        for (int i = 0; i < nearbyEnvironment.Count; i++)
        {
            nearbyEnvironment[i].SetTrigger("Revive");
        }
    }

    protected override void DestroyDefence()
    {
        if (isDestroyed) return;
        base.DestroyDefence();

        for (int i = 0; i < nearbyEnvironment.Count; i++)
        {
            nearbyEnvironment[i].SetTrigger("Break");
        }

        for (int i = 0; i < wallStats.wallHolderParts.Count; i++)
        {
            wallStats.wallHolderParts[i].SetActive(false);
        }
    }

    #endregion

}
