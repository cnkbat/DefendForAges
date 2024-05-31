using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : AttackerDefenceBehaviour
{
    NearestEnemyFinder nearestEnemyFinder;
    TowerStats towerStats;

    [Header("Events")]
    public Action OnTowerDestroyed;

    [Header("Recovery")]
    [SerializeField] private float recoveryTimer;
    private float currentRecoveryTimer;

    override protected void OnEnable()
    {
        base.OnEnable();
        towerStats = GetComponent<TowerStats>();

        for (int i = 0; i < towerStats.GetWeapons().Count; i++)
        {
            OnRangedAttack += towerStats.GetWeapons()[i].Attack;
        }

        ResetRecoveryTimer();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        for (int i = 0; i < towerStats.GetWeapons().Count; i++)
        {
            OnRangedAttack -= towerStats.GetWeapons()[i].Attack;
        }
    }

    override protected void Start()
    {
        base.Start();

        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();
        ResetHealthValue();
    }

    protected override void Update()
    {
        base.Update();


        if (currentRecoveryTimer <= 0)
        {
            if (currentHealth <= towerStats.GetMaxHealth())
            {
                currentHealth += towerStats.GetRecovery();
            }
        }

    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        ResetRecoveryTimer();
        // haptic oynat
        // feeli ver.
    }

    private void ResetRecoveryTimer()
    {
        currentRecoveryTimer = recoveryTimer;
    }

    protected override void Attack()
    {
        base.Attack();

        if (!nearestEnemyFinder.GetNearestEnemy()) return;

        OnRangedAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy(), towerStats.GetDamage(), false);
    }

    protected override void DestroyDefence()
    {
        if(isDestroyed) return;
        
        base.DestroyDefence();
        // ağır haptic oynat
        OnTowerDestroyed?.Invoke();
    }





}
