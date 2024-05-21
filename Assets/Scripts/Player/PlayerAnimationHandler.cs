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
    public Action<float> OnAttackAnimSpeedSet;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        playerStats = GetComponent<PlayerStats>();
        playerAngleCalculator = GetComponent<PlayerAngleCalculator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnEnable()
    {
        playerStats.OnMovementSpeedUpgraded += ChangeWalkAS;
        playerStats.OnAttackSpeedUpgraded += ChangeAttackAS;

        playerAngleCalculator.OnPlayerMoved += SetWalkAnimationValues;

        OnAttackAnimSpeedSet += playerAttack.SetAttackingDelay;
        playerAttack.OnAttackAnimPlayNeeded += SetAttackTrigger;
    }

    private void OnDisable()
    {

        playerStats.OnMovementSpeedUpgraded -= ChangeWalkAS;
        playerStats.OnAttackSpeedUpgraded -= ChangeAttackAS;

        playerAngleCalculator.OnPlayerMoved -= SetWalkAnimationValues;

        OnAttackAnimSpeedSet -= playerAttack.SetAttackingDelay;
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

        Debug.Log("Vx = " + vectorX + " Vy = " + vectorY);
    }

    #endregion

    #region Attacking

    public void SetAttackTrigger()
    {
        animator.SetTrigger("Attack");
    }

    private void ChangeAttackAS()
    {
        animator.SetFloat("AttackAnimationSpeed", playerStats.GetAttackSpeed() * attackASMultiplier);

        OnAttackAnimSpeedSet?.Invoke(attackASMultiplier);
    }

    #endregion

}
