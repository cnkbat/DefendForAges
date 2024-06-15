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

        wallStats.OnBuyDone += ReviveTarget;
        playerStats.OnWaveWon += CheckForUpgradeable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        wallStats.OnBuyDone -= ReviveTarget;
        playerStats.OnWaveWon -= CheckForUpgradeable;
    }

    override protected void Start()
    {
        base.Start();
    }
    private void Update()
    {
        // Revive defence system needed
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
                // wallStats.wallParts[i].SetActive(false);
                BreakPart(wallStats.wallParts[i]);
            }
        }
    }
    public void BreakPart(GameObject part){
        // when defence is destroyed, one by one parts will be broken with this function
        // first, activate gravity on rigidbody.
        // launch the rigidbody with addForce (towards the base, not outside)
        // don't deactivate the object, it will be repaired at the end of the wave and go back to its place. (logic missing for this)

    }

    #region Repair Related

    public override void ReviveTarget()
    {
        base.ReviveTarget();

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

    // when defence is repaired, one by one parts will be put back with this function
    public void RepairPart(){

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

    public override void CheckForUpgradeable()
    {
        base.CheckForUpgradeable();

        if(!isRepairable)
        {
            defencesStatsBase.SetLoadableBaseActivity(false);
        }
        
    }

}
