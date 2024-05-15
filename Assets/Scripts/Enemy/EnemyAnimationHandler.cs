using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyBehaviour enemyBehaviour;
    [Header("Events")]
    public Action OnAttacking;
    public Action OnTargetNotReached;
    public Action OnDeath;
    public Action OnMove;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        OnAttacking += Attack;
        OnTargetNotReached += StopAttack;
        OnMove += MoveAnimation;
        OnDeath += Death;
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
}
