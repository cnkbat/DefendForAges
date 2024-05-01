using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesBehaviourBase : EnemyTarget
{
    protected DefencesStatsBase defencesStatsBase;
    protected float currentHealth;

    void Start()
    {
        defencesStatsBase = GetComponent<DefencesStatsBase>();
        ResetHealthValue();
    }

    protected virtual void ResetHealthValue()
    {
        currentHealth = defencesStatsBase.GetMaxHealth();
    }

    public override void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
    }
}
