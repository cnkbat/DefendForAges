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
        // Revive defence system needed
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
                wallStats.wallParts[i].SetActive(false);
                // Debug.Log("Breaking part " + i);
                // var part = wallStats.wallParts[i].GetComponent<WallPartBehaviour>();
                // if(!part.broken)
                //     part.BreakPart();
            }
        }
    }
    // public void BreakPart(GameObject part){
    //     // when defence is destroyed, one by one parts will be broken with this function
    //     // this function is repeated for EVERY PART, everytime a new part will be broken. So first part goes through this function 6 times in total.
    //     // ^^ looks good because it looks like parts are blown away when last part is broken with wall breakdown animation


    //     // first, activate gravity on rigidbody.
    //     Rigidbody rb = part.GetComponent<Rigidbody>();
    //     rb.useGravity = true;
    //     // then, unfreeze the rigidbodys movement
    //     rb.constraints = RigidbodyConstraints.None;
    //     // freeze it back after two seconds
    //     StartCoroutine(FreezeRigidbody(rb));
    //     // launch the rigidbody with addForce (towards the base, not outside)(inside is -z direction)
    //     rb.AddForce(-transform.forward * 20, ForceMode.Impulse);
    //     // don't deactivate the object, it will be repaired at the end of the wave and go back to its place. (logic missing for this)

    // }
    // IEnumerator FreezeRigidbody(Rigidbody rb){
    //     yield return new WaitForSeconds(2);
    //     rb.constraints = RigidbodyConstraints.FreezeAll;
    // }

    #region Repair Related

    public override void ReviveTarget()
    {
        base.ReviveTarget();

        for (int i = 0; i< wallStats.wallParts.Count; i++)
        {
            wallStats.wallParts[i].SetActive(true);
            // Debug.Log("Repairing part " + i);
            // // RepairPart(wallStats.wallParts[i], i);
            // var part = wallStats.wallParts[i].GetComponent<WallPartBehaviour>();
            // if(part.broken)
            //     part.RepairPart();
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
    // public void RepairPart(GameObject part, int index){
    //     // get location from transform list in wallStats
    //     // bug on 121 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //     Vector3 pos = wallStats.wallPartLocations[index][0];
    //     Vector3 rot = wallStats.wallPartLocations[index][1];
    //     // unfreeze the rigidbody
    //     Rigidbody rb = part.GetComponent<Rigidbody>();
    //     rb.constraints = RigidbodyConstraints.None;
    //     // move part to that location with DOmove and Dorotate. Freeze after done.
    //     part.transform.DOMove(pos, 2).SetEase(Ease.OutQuad).OnComplete(() => rb.constraints = RigidbodyConstraints.FreezeAll);
    //     part.transform.DORotate(rot, 2).SetEase(Ease.OutQuad).OnComplete(() => rb.constraints = RigidbodyConstraints.FreezeAll);
    // }
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
