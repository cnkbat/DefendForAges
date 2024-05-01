using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : DefencesBehaviourBase
{
    TowerStats towerStats;
    public Action OnTowerDestroyed;

    void Start()
    {
        towerStats = GetComponent<TowerStats>();
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
        if (isDestroyed)
        {
            // ağır haptic oynat
            return;
        }
        // haptic oynat
        // feeli ver.
    }

    protected override void DestroyDefence()
    {
        base.DestroyDefence();

        OnTowerDestroyed?.Invoke();
    }



}
