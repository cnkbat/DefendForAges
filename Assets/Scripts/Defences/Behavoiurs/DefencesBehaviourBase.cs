using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesBehaviourBase : MonoBehaviour
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

    public virtual void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
    }
}
