using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WallBehaviour : DefencesBehaviourBase
{

    WallStats wallStats;

    [SerializeField] private List<Animator> nearbyEnvironment;

    protected override void OnEnable()
    {
        base.OnEnable();
        wallStats = GetComponent<WallStats>();
        // if setting transforms publicly does not work as intended, here transforms will be cloned and used as starting pos/rotations
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
        if(Input.GetKeyDown(KeyCode.P))
        { TakeDamage(10); }
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
        // this function is repeated for EVERY PART, everytime a new part will be broken. So first part goes through this function 6 times in total.
        // ^^ looks good because it looks like parts are blown away when last part is broken with wall breakdown animation


        // first, activate gravity on rigidbody.
        Rigidbody rb = part.GetComponent<Rigidbody>();
        rb.useGravity = true;
        // then, unfreeze the rigidbodys movement
        rb.constraints = RigidbodyConstraints.None;
        // freeze it back after two seconds
        StartCoroutine(FreezeRigidbody(rb));
        // launch the rigidbody with addForce (towards the base, not outside)(inside is -z direction)
        rb.AddForce(-transform.forward * 20, ForceMode.Impulse);
        // don't deactivate the object, it will be repaired at the end of the wave and go back to its place. (logic missing for this)

    }
    IEnumerator FreezeRigidbody(Rigidbody rb){
        yield return new WaitForSeconds(2);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    #region Repair Related

    public override void ReviveTarget()
    {
        base.ReviveTarget();

        for (int i = 0; i< wallStats.wallParts.Count; i++)
        {
            RepairPart(wallStats.wallParts[i]);
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

    // when defence is repaired, one by one parts will be put back with this function
    public void RepairPart(GameObject part){
        // get location from transform list in wallStats
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
