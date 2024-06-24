using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private EnemyDeathHandler enemyDeathHandler;
    private EnemyAttack enemyAttack;
    private EnemyMovement enemyMovement;

    [Header("Animation Speed")]
    [Tooltip("Çarpan olarak çalışıyor direkt 0.5 yazarsan ikiye bölünür.")][SerializeField] private float walkAnimMultiplier = 1;
    [Tooltip("Çarpan olarak çalışıyor direkt 0.5 yazarsan ikiye bölünür.")][SerializeField] private float attackSpeedMultiplier = 1;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        enemyDeathHandler = GetComponent<EnemyDeathHandler>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void OnEnable()
    {
        ResetAnimator();

        enemyAttack.OnAttacking += Attack;
        enemyAttack.OnTargetNotReached += StopAttack;
        enemyMovement.OnMove += MoveAnimation;

        // Anim speed change
        enemyMovement.OnMovementSpeedChanged += ChangeWalkAnimationSpeed;
        enemyAttack.OnAttackSpeedChanged += ChangeAttackAnimationSpeed;
    }

    private void OnDisable()
    {
        enemyAttack.OnAttacking -= Attack;
        enemyAttack.OnTargetNotReached -= StopAttack;
        enemyMovement.OnMove -= MoveAnimation;

        // Anim speed change
        enemyMovement.OnMovementSpeedChanged -= ChangeWalkAnimationSpeed;
        enemyAttack.OnAttackSpeedChanged -= ChangeAttackAnimationSpeed;
    }


    private void ResetAnimator()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);
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
        animator.SetBool("isWalking", enemyMovement.GetCanMove());
    }



    #region Anim Speed Change

    public void ChangeWalkAnimationSpeed(float newSpeed)
    {
        animator.SetFloat("WalkASMultiplier", newSpeed * walkAnimMultiplier);
    }

    public void ChangeAttackAnimationSpeed(float newSpeed)
    {
        // might need to add a check for newspeed == 0
        animator.SetFloat("AttackASMultiplier", attackSpeedMultiplier / newSpeed);
    }

    #endregion

}
