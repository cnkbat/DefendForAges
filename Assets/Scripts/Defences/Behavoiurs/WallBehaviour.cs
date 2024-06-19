using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class WallBehaviour : DefencesBehaviourBase
{

    WallStats wallStats;

    [SerializeField] private List<Animator> nearbyEnvironment;

    protected override void OnEnable()
    {
        base.OnEnable();
        wallStats = GetComponent<WallStats>();
        // if setting transforms publicly does not work as intended, here transforms will be cloned and used as starting pos/rotations
        // for(int i = 0; i < wallStats.wallParts.Count; i++)
        // {
        //     // cant clone transforms without creating new objects, had to use a 2d list
        //     // wallStats.wallParts[i].transform.position, wallStats.wallParts[i].transform.rotation
        //     List<Vector3> pos_rot = new List<Vector3>();
        //     Vector3 pos = wallStats.wallParts[i].transform.position;
        //     Vector3 rot = wallStats.wallParts[i].transform.rotation.eulerAngles;
        //     pos_rot.Add(pos);
        //     pos_rot.Add(rot);
        //     wallStats.wallPartLocations.Add(pos_rot);
        // }
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
        // Break test
        if(Input.GetKeyDown(KeyCode.O))
        { TakeDamage(10); }
        // Revive defence
        if (Input.GetKeyDown(KeyCode.P))
        { ReviveTarget(); }

        // if (Input.GetKeyDown(KeyCode.O))
        // { DestroyDefence(); }
    }
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        for (int i = 0; i < wallStats.healthParts.Count; i++)
        {
            if (currentHealth < wallStats.healthParts[i])
            {
                var part = wallStats.wallParts[i].GetComponent<WallPartBehaviour>();
                if(!part.broken)
                    part.BreakPart(-transform.forward);
            }
        }
    }

    #region Repair Related

    public override void ReviveTarget()
    {
        base.ReviveTarget();

        for (int i = 0; i< wallStats.wallParts.Count; i++)
        {
            var part = wallStats.wallParts[i].GetComponent<WallPartBehaviour>();
            if(part.broken)
                part.RepairPart();
        }
        for (int i = 0; i < wallStats.wallHolderParts.Count; i++)
        {
            // could add an animation here, something like they go up from ground and slowly materialise (transparency)
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

    public override void CheckForUpgradeable()
    {
        base.CheckForUpgradeable();

        if(!isRepairable)
        {
            defencesStatsBase.SetLoadableBaseActivity(false);
        }
        
    }

}
