using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesBehaviourBase : EnemyTarget
{
    protected DefencesStatsBase defencesStatsBase;
    

    override protected void Start()
    {
        defencesStatsBase = GetComponent<DefencesStatsBase>();
        ResetHealthValue();
    }

    public override void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

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
        // hapticler
    }
}
