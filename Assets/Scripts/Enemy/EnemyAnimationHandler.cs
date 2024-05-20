using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private EnemyBehaviour enemyBehaviour;

    [Header("Animation Speed")]
    [Tooltip("Çarpan olarak çalışıyor direkt 0.5 yazarsan ikiye bölünür.")][SerializeField] private float walkAnimMultiplier = 1;
    [Tooltip("Çarpan olarak çalışıyor direkt 0.5 yazarsan ikiye bölünür.")][SerializeField] private float attackSpeedMultiplier = 1;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }
    private void OnEnable()
    {

        enemyBehaviour.OnAttacking += Attack;
        enemyBehaviour.OnTargetNotReached += StopAttack;
        enemyBehaviour.OnMove += MoveAnimation;
        enemyBehaviour.OnDeath += Death;

        // Anim speed change
        enemyBehaviour.OnMovementSpeedChanged += ChangeWalkAnimationSpeed;
        enemyBehaviour.OnAttackSpeedChanged += ChangeAttackAnimationSpeed;
    }

    private void OnDisable()
    {
        enemyBehaviour.OnAttacking -= Attack;
        enemyBehaviour.OnTargetNotReached -= StopAttack;
        enemyBehaviour.OnMove -= MoveAnimation;
        enemyBehaviour.OnDeath -= Death;


        // Anim speed change
        enemyBehaviour.OnMovementSpeedChanged -= ChangeWalkAnimationSpeed;
        enemyBehaviour.OnAttackSpeedChanged -= ChangeAttackAnimationSpeed;
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


    #region Anim Speed Change

    public void ChangeWalkAnimationSpeed(float newSpeed)
    {
        animator.SetFloat("WalkASMultiplier", newSpeed / walkAnimMultiplier);
    }

    public void ChangeAttackAnimationSpeed(float newSpeed)
    {
        animator.SetFloat("AttackASMultiplier", newSpeed / attackSpeedMultiplier);
    }

    #endregion
}
