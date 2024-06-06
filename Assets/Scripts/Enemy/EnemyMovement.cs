using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyTargeter enemyTargeter;
    private EnemyStats enemyStats;
    private EnemyDeathHandler enemyDeathHandler;
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    private GameManager gameManager;

    [Header("State")]
    [SerializeField] private bool canMove;

    [Header("Movement Related Events")]
    public Action OnMove;
    public Action<float> OnMovementSpeedChanged;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyTargeter = GetComponent<EnemyTargeter>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<EnemyStats>();
        enemyDeathHandler = GetComponent<EnemyDeathHandler>();
    }

    public void EnemySpawned()
    {
        gameManager = GameManager.instance;

        OnMove?.Invoke();

        canMove = true;
    }

    public void Update()
    {
        rb.velocity = Vector3.zero;

        if (enemyDeathHandler.GetIsDead()) return;

        if (enemyTargeter.GetTargetTransform() == null) return;

        Move();
    }

    #region  Movement

    public IEnumerator EnableMovement(float duration)
    {
        if (enemyDeathHandler.GetIsDead()) yield return null;

        yield return new WaitForSeconds(duration);

        if (enemyDeathHandler.GetIsDead()) yield return null;

        navMeshAgent.speed = enemyStats.GetMovementSpeed();
    }

    public void Move()
    {
        bool isStoppedBool = canMove & !gameManager.isGameFreezed;

        navMeshAgent.isStopped = !isStoppedBool;
        OnMove?.Invoke();

        OnMovementSpeedChanged?.Invoke(navMeshAgent.speed);

        if (enemyTargeter.GetTargetTransform() != null)
        {
            navMeshAgent.destination = enemyTargeter.GetTargetTransform().position;
        }
        
    }


    //// Oyun hissiyatı izlenecek ve duruma göre geri getirelecek.

    /*  public void GetRandomPosition(Transform newTarget)
      {

          Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * gameManager.samplePositionRadius;
          randomDirection += newTarget.transform.position;
          NavMeshHit hit;

          NavMesh.SamplePosition(randomDirection, out hit, gameManager.samplePositionRadius, gameManager.surfaceAreaIndex);
          target = hit.position;

          if (target.x == Mathf.Infinity)
          {
              GetRandomPosition(newTarget);
          }

      } */

    #endregion


    #region Getters & Setters

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetCanMove(bool newCanMove)
    {
        canMove = newCanMove;
    }

    #endregion 

}
