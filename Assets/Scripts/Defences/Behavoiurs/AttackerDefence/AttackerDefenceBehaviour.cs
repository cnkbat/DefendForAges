using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerDefenceBehaviour : DefencesBehaviourBase
{

    protected AttackerDefenceStat attackerDefenceStat;

    [Header("Combat")]
    protected float currentAttackSpeed;

    [Header("Ranged Event")]
    public Action<EnemyDeathHandler, float, bool> OnRangedAttack;

    protected override void OnEnable()
    {
        attackerDefenceStat = GetComponent<AttackerDefenceStat>();
        base.OnEnable();
        ResetAttackSpeed();

        playerStats.OnWaveWon += CheckForUpgradeable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        playerStats.OnWaveWon -= CheckForUpgradeable;
    }


    protected override void Start()
    {
        base.Start();

    }

    protected virtual void Update()
    {
        if (isDestroyed) return;
        if (gameManager.isGameFreezed) return;

        if (!gameManager.isAttackPhase)
        {
            ResetAttackSpeed();
            return;
        }

        currentAttackSpeed -= Time.deltaTime;

        if (currentAttackSpeed <= 0)
        {
            Attack();
        }

    }

    protected virtual void Attack()
    {
        ResetAttackSpeed();
    }

    protected virtual void ResetAttackSpeed()
    {
        currentAttackSpeed = attackerDefenceStat.GetAttackSpeed();
    }


}
