using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : AttackerDefenceBehaviour
{
    NearestEnemyFinder nearestEnemyFinder;
    TowerStats towerStats;
    public Action OnTowerDestroyed;

    public Action<Transform, float> OnAttack;

    override protected void Start()
    {
        base.Start();
        
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();
        towerStats = GetComponent<TowerStats>();
        ResetHealthValue();
    }

    protected override void Update()
    {
        if (currentHealth <= towerStats.GetMaxHealth())
        {
            currentHealth += towerStats.GetRecovery();
        }
        base.Update();
    }
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);


        // haptic oynat
        // feeli ver.
    }
    protected override void Attack()
    {
        base.Attack();

        if (!nearestEnemyFinder.GetNearestEnemy()) return;

        OnAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy(), towerStats.GetDamage());
    }
    protected override void DestroyDefence()
    {
        base.DestroyDefence();
        // ağır haptic oynat
        OnTowerDestroyed?.Invoke();
    }





}
