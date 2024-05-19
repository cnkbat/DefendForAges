using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private EnemyBehaviour enemyBehaviour;

    private void OnEnable()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        enemyBehaviour.OnAttacking += Attack;
        enemyBehaviour.OnTargetNotReached += StopAttack;
        enemyBehaviour.OnMove += MoveAnimation;
        enemyBehaviour.OnDeath += Death;
        enemyBehaviour.OnMovementSpeedChanged += ChangeWalkAnimationSpeed;
    }

    private void OnDisable()
    {
        enemyBehaviour.OnAttacking -= Attack;
        enemyBehaviour.OnTargetNotReached -= StopAttack;
        enemyBehaviour.OnMove -= MoveAnimation;
        enemyBehaviour.OnDeath -= Death;
    }

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Attack()
    {
        animator.SetTrigger("Attacking");
    }
    public void StopAttack()
    {
        animator.SetBool("isAttacking", false);
    }
    public void MoveAnimation()
    {
        animator.SetBool("isWalking", enemyBehaviour.GetCanMove());
    }
    public void Death()
    {
        animator.SetBool("isKill", true);
    }

    public void ChangeWalkAnimationSpeed(float newSpeed)
    {
        animator.SetFloat("WalkSpeed", newSpeed);
    }
}
