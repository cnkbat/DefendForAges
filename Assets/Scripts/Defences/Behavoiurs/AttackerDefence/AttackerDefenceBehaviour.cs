using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerDefenceBehaviour : DefencesBehaviourBase
{
   
    protected AttackerDefenceStat attackerDefenceStat;

    [Header("Combat")]
    protected float currentAttackSpeed;

    protected override void OnEnable()
    {
        attackerDefenceStat = GetComponent<AttackerDefenceStat>();

        base.OnEnable();
        ResetAttackSpeed();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
    {
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
