using System;
using MoreMountains.Feedbacks;
using UnityEngine;

public class DefencesBehaviourBase : EnemyTarget
{

    protected DefencesStatsBase defencesStatsBase;

    BoxCollider boxCollider;
    [SerializeField] protected bool isRepairable;

    [Header("Events")]
    public Action<bool> OnRepairStateChange;

    [Header("Visuals")]
    [SerializeField] protected GameObject asset;
    [SerializeField] protected MMFeedbacks feelFeedBacks;

    override protected void Start()
    {
        defencesStatsBase = GetComponent<DefencesStatsBase>();
        boxCollider = GetComponent<BoxCollider>();

        ResetHealthValue();
        CheckForUpgradeable();
    }

    public override void TakeDamage(float dmg)
    {


        base.TakeDamage(dmg);

        if (feelFeedBacks != null)
        {
            feelFeedBacks?.PlayFeedbacks();
        }

        currentHealth -= dmg;

        SetisRepairable(true);

        if (currentHealth <= 0 && !isDestroyed)
        {
            DestroyDefence();
        }
    }

    public override void ResetHealthValue()
    {
        currentHealth = defencesStatsBase.GetMaxHealth();
    }

    protected virtual void DestroyDefence()
    {
        if(isDestroyed) return;

        base.TargetDestroyed();

        // animasyon gelince ayarlanacak.
        //asset.SetActive(false);
        boxCollider.enabled = false;

        // hapticler
    }

    #region Repair
    public override void TargetRevived()
    {

        base.TargetRevived();
        if (!isRepairable) return;

        asset.SetActive(true);
        boxCollider.enabled = true;

        ResetHealthValue();
        SetisRepairable(false);
        CheckForUpgradeable();

    }

    public void CheckForUpgradeable()
    {
        defencesStatsBase.SetLoadableBaseActivity(isRepairable);
    }

    #endregion
    #region  Getters & Setters
    private void SetisRepairable(bool newBool)
    {
        OnRepairStateChange.Invoke(newBool);

        isRepairable = newBool;
    }
    #endregion
}
