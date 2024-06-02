using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    protected EnemyTargeter enemyTargeter;
    protected EnemyStats enemyStats;
    protected EnemyMovement enemyMovement;
    private NavMeshAgent navMeshAgent;

    [Header("AI")]
    [SerializeField] private float originalStoppingDistance;

    [Header("Aim")]
    [SerializeField] private float aimSpeed = 20f;

    [Header("Combat")]
    private float attackTimer;

    [Header("Visuals")]
    [SerializeField] private Transform enemyAsset;

    [Header("Events")]
    public Action OnAttacking;
    public Action<float> OnAttackSpeedChanged;
    public Action OnTargetNotReached;

    private void Awake()
    {
        enemyAsset = transform.Find("enemyAsset");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        enemyStats = transform.GetComponent<EnemyStats>();
        enemyTargeter = transform.GetComponent<EnemyTargeter>();
        enemyMovement = transform.GetComponent<EnemyMovement>();
    }

    public void EnemySpawned()
    {
        ResetAttackSpeed();
        navMeshAgent.stoppingDistance = originalStoppingDistance;
    }



    private void Update()
    {
        // if (isDead) return;

        if (enemyTargeter.GetTargetTransform() == null) return;

        LookAtTarget();
        Attacking();
    }


    #region Attacking

    private void LookAtTarget()
    {
        Vector3 worldAimTarget = enemyTargeter.GetTargetTransform().transform.position;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, aimSpeed * Time.deltaTime);

        enemyAsset.forward = transform.forward;

        enemyAsset.localPosition = Vector3.zero;
    }

    private void Attacking()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("OnlyPlayer");

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, enemyStats.GetRange(), layerMask))
        {
            if (hit.transform.TryGetComponent(out EnemyTarget enemyTarget))
            {
                navMeshAgent.stoppingDistance = enemyTarget.GetStoppingDistance();
                attackTimer -= Time.deltaTime;
                enemyMovement.SetCanMove(false);

                if (attackTimer <= 0)
                {
                    OnAttacking?.Invoke();
                    ResetAttackSpeed();
                }
            }
            else
            {
                OnTargetNotReached?.Invoke();
                enemyMovement.SetCanMove(true);

                navMeshAgent.stoppingDistance = originalStoppingDistance;
            }
        }
        else
        {
            OnTargetNotReached?.Invoke();
            enemyMovement.SetCanMove(true);

            navMeshAgent.stoppingDistance = originalStoppingDistance;
        }

    }


    public void ResetAttackSpeed()
    {
        attackTimer = enemyStats.GetAttackSpeed();
        OnAttackSpeedChanged?.Invoke(enemyStats.GetAttackSpeed());
    }

    #endregion

}
