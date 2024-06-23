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
    protected EnemyDeathHandler enemyDeathHandler;
    private NavMeshAgent navMeshAgent;
    protected ObjectPooler objectPooler;

    [Header("AI")]
    [SerializeField] private float originalStoppingDistance;

    [Header("Aim")]
    [SerializeField] private float aimSpeed = 20f;

    [Header("Combat")]
    [SerializeField] private Transform raycastPos;
    private float attackTimer;

    [Header("Visuals")]
    [SerializeField] private Transform enemyAsset;

    [Header("Events")]
    public Action OnAttacking;
    public Action<EnemyTarget> OnAttackedObjectSet;
    public Action<float> OnAttackSpeedChanged;
    public Action OnTargetNotReached;

    private void Awake()
    {
        enemyAsset = transform.Find("enemyAsset");
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        enemyStats = this.GetComponent<EnemyStats>();
        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyMovement = this.GetComponent<EnemyMovement>();
        enemyDeathHandler = this.GetComponent<EnemyDeathHandler>();
    }

    public void EnemySpawned()
    {
        ResetAttackSpeed();
        navMeshAgent.stoppingDistance = originalStoppingDistance;
    }

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
    }

    private void Update()
    {
        if (enemyDeathHandler.GetIsDead()) return;

        if (enemyTargeter.GetTargetTransform() == null)
        {
            Debug.Log("target null");
            return;
        }

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
        int layerMask = ~LayerMask.GetMask("Enemy");

        if (Physics.Raycast(raycastPos.position, transform.forward, out hit, enemyStats.GetRange(), layerMask))
        {
            Debug.DrawRay(raycastPos.position, transform.forward, Color.green, 0.5f);

            if (hit.transform.TryGetComponent(out EnemyTarget enemyTarget))
            {
                navMeshAgent.stoppingDistance = enemyTarget.GetStoppingDistance();
                attackTimer -= Time.deltaTime;
                enemyMovement.SetCanMove(false);

                if (attackTimer <= 0)
                {
                    OnAttacking?.Invoke();
                    OnAttackedObjectSet?.Invoke(enemyTarget);

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
