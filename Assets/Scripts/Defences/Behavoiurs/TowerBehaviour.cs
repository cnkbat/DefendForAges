using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : DefencesBehaviourBase
{
    TowerStats towerStats;
    public Action OnTowerDestroyed;

    override protected void Start()
    {
        base.Start();
        towerStats = GetComponent<TowerStats>();
        ResetHealthValue();
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);


        // haptic oynat
        // feeli ver.
    }

    protected override void DestroyDefence()
    {
        base.DestroyDefence();
        // ağır haptic oynat
        OnTowerDestroyed?.Invoke();
    }





}
