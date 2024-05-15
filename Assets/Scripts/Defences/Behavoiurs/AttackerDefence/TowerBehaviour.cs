using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : AttackerDefenceBehaviour
{
    NearestEnemyFinder nearestEnemyFinder;
    TowerStats towerStats;
    [SerializeField] float towerAttackingAnimDur;

    [Header("Events")]
    public Action OnTowerDestroyed;

    override protected void OnEnable()
    {
        base.OnEnable();
        towerStats = GetComponent<TowerStats>();

        for (int i = 0; i < towerStats.GetWeapons().Count; i++)
        {
            OnRangedAttack += towerStats.GetWeapons()[i].Attack;
        }
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

        OnRangedAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy(), towerStats.GetDamage(), false, towerAttackingAnimDur);
    }

    protected override void DestroyDefence()
    {
        base.DestroyDefence();
        // ağır haptic oynat
        OnTowerDestroyed?.Invoke();
    }





}
