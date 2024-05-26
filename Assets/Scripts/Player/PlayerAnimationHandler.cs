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
    }

    private void OnEnable()
    {
        playerStats.OnMovementSpeedUpgraded += ChangeWalkAS;
        playerStats.OnAttackSpeedUpgraded += ChangeAttackAS;

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
        animator.SetTrigger("Attack");
    }

    public void AttackAtEnemy()
    {
        OnAnimEventFired?.Invoke();
    }

    private void ChangeAttackAS()
    {
        // might need to add a check for getAttackSpeed() == 0
        animator.SetFloat("AttackAnimationSpeed", attackASMultiplier / playerStats.GetAttackSpeed());
    }

    #endregion

}
