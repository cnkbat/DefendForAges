using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [Header("Components")]
    Animator animator;
    private PlayerStats playerStats;
    private PlayerAngleCalculator playerAngleCalculator;
    private PlayerAttack playerAttack;
    private PlayerDeathHandler playerDeathHandler;

    [Header("Animation Speed Changers")]
    [SerializeField] private float walkASMultiplier;
    [SerializeField] private float attackASMultiplier;

    [Header("Actions")]
    public Action OnAnimEventFired;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerStats = transform.parent.GetComponent<PlayerStats>();
        playerAngleCalculator = transform.parent.GetComponent<PlayerAngleCalculator>();
        playerAttack = transform.parent.GetComponent<PlayerAttack>();
        playerDeathHandler = transform.parent.GetComponent<PlayerDeathHandler>();
    }

    private void OnEnable()
    {
        playerStats.OnMovementSpeedUpgraded += ChangeWalkAS;
        playerStats.OnAttackSpeedUpgraded += ChangeAttackAS;
        playerDeathHandler.OnTargetDestroyed += SetDeathAnim;
        playerStats.OnRevivePlayer += SetReviveAnim;

        playerAngleCalculator.OnPlayerMoved += SetWalkAnimationValues;

        playerAttack.OnAttackAnimPlayNeeded += SetAttackTrigger;

        OnAnimEventFired += playerAttack.Attack;

        // TO TEST OUT WALK ANIMATION SPEED
        ChangeWalkAS();
        ChangeAttackAS();
    }

    private void OnDisable()
    {

        playerStats.OnMovementSpeedUpgraded -= ChangeWalkAS;
        playerStats.OnAttackSpeedUpgraded -= ChangeAttackAS;

        playerDeathHandler.OnTargetDestroyed -= SetDeathAnim;
        playerStats.OnRevivePlayer -= SetReviveAnim;

        playerAttack.OnAttackAnimPlayNeeded -= SetAttackTrigger;
        playerAngleCalculator.OnPlayerMoved -= SetWalkAnimationValues;

        OnAnimEventFired -= playerAttack.Attack;

    }

    #region Walking

    private void ChangeWalkAS()
    {
        animator.SetFloat("WalkSpeed", playerStats.GetMovementSpeed() * walkASMultiplier);
    }

    private void SetWalkAnimationValues(float vectorX, float vectorY)
    {
        animator.SetFloat("Vx", vectorX);
        animator.SetFloat("Vy", vectorY);
    }

    #endregion

    #region Attacking

    public void SetAttackTrigger()
    {
        if (playerDeathHandler.GetIsDead()) return;
        animator.SetTrigger("Attack");
    }

    public void AttackAtEnemy()
    {
        if (playerDeathHandler.GetIsDead()) return;
        OnAnimEventFired?.Invoke();
    }

    private void ChangeAttackAS()
    {
        // might need to add a check for getAttackSpeed() == 0
        animator.SetFloat("AttackAnimationSpeed", attackASMultiplier / playerStats.GetAttackSpeed());
    }

    #endregion

    private void SetDeathAnim()
    {
        animator.SetBool("isDead", true);
    }
    private void SetReviveAnim()
    {
        animator.SetBool("isDead", false);
    }
}
