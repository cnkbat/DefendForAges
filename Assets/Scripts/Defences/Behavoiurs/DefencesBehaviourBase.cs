using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesBehaviourBase : EnemyTarget
{
    protected DefencesStatsBase defencesStatsBase;
    [SerializeField] protected GameObject asset;
    BoxCollider boxCollider;
    [SerializeField] protected bool isRepairable;

    override protected void Start()
    {
        defencesStatsBase = GetComponent<DefencesStatsBase>();
        boxCollider = GetComponent<BoxCollider>();
        ResetHealthValue();
        CheckForUpgradeable();
    }

    public override void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        SetisRepairable(true);

        if (currentHealth <= 0)
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
        base.TargetDestroyed();
        asset.SetActive(false);
        boxCollider.enabled = false;
    
        // hapticler
    }

    #region Repair
    public override void TargetRevived()
    {

        if (!isRepairable) return;

        base.TargetRevived();

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
        isRepairable = newBool;
    }
    #endregion
}
