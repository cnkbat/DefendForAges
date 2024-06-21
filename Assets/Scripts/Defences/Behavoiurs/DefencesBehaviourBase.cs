using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;

public class DefencesBehaviourBase : EnemyTarget
{

    protected DefencesStatsBase defencesStatsBase;

    protected NavMeshObstacle navMeshObstacle;
    BoxCollider boxCollider;
    [SerializeField] protected bool isRepairable;

    [Header("Is Tower?")]
    [SerializeField] protected bool isTower;

    [Header("Events")]
    public Action<bool> OnRepairStateChange;
    public Action OnRepairDone;

    [Header("Visuals")]

    [SerializeField] protected MMFeedbacks feelFeedBacks;

    override protected void Start()
    {
        defencesStatsBase = GetComponent<DefencesStatsBase>();
        boxCollider = GetComponent<BoxCollider>();


        if (TryGetComponent(out NavMeshObstacle obs))
        {
            navMeshObstacle = obs;
        }

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

        OnDamageTaken?.Invoke();
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
        if (isDestroyed) return;

        base.TargetDestroyed();


        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = false;
        }


        // animasyon gelince ayarlanacak.
        boxCollider.enabled = false;

        // hapticler
    }

    #region Repair
    public override void ReviveTarget()
    {

        base.ReviveTarget();

        if (!isRepairable) return;

        boxCollider.enabled = true;

        if (navMeshObstacle != null)
        {
            navMeshObstacle.enabled = true;
        }


        OnRepairDone?.Invoke();
        ResetHealthValue();
        SetisRepairable(false);
        CheckForUpgradeable();

    }

    public virtual void CheckForUpgradeable()
    {
        if (isRepairable)
        {
            defencesStatsBase.SetLoadableBaseActivity(true);
            return;
        }

    }

    #endregion
    #region  Getters & Setters

    private void SetisRepairable(bool newBool)
    {

        if (isTower)
        {
            isRepairable = false;
            return;
        }
        else
        {
            Debug.Log("set repair");
            OnRepairStateChange.Invoke(newBool);

            isRepairable = newBool;
        }

    }

    public bool GetIsRepariable()
    {
        return isRepairable;
    }

    #endregion
}
