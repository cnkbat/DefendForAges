using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerStats playerStats;
    GameManager gameManager;
    ObjectPooler objectPooler;

    [Header("Nearest Enemy Finding")]
    private float nearestEnemyDistance;
    float distance;
    [SerializeField] Transform nearestEnemy;

    [Tooltip("Attack Speed")]
    float currentAttackSpeed;

    [Tooltip("Events")]
    public Action<Transform> OnAttack;

    private void Start()
    {
        gameManager = GameManager.instance;
        objectPooler = ObjectPooler.instance;
        playerStats = GetComponent<PlayerStats>();
        ResetAttackSpeed();
    }

    private void Update()
    {
        if (GetNearestEnemyToPlayer())
        {
            currentAttackSpeed -= Time.deltaTime;
            if (currentAttackSpeed <= 0)
            {
                OnAttack?.Invoke(GetNearestEnemyToPlayer());
                ResetAttackSpeed();
            }
        }
        else
        {
            ResetAttackSpeed();
        }
    }

    #region  findingClosestEnemy

    void LookAtNearstEnemy(Transform closestEnemy)
    {

        if (closestEnemy != null)
        {
            //[SerializeField] Transform headAimObject;
            // anim rigging yapcaz ? belki
            //  headAimObject.transform.parent = closestEnemy;
            //headAimObject.transform.localPosition = Vector3.zero;


            Vector3 worldAimTarget = closestEnemy.transform.position;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = aimDirection;

        }

    }

    public Transform GetNearestEnemyToPlayer()
    {

        nearestEnemyDistance = 10000f;

        if (gameManager.allSpawnedEnemies.Count > 0)
        {

            for (int i = 0; i < gameManager.allSpawnedEnemies.Count; i++)
            {
                distance = Vector3.Distance(transform.position, gameManager.allSpawnedEnemies[i].transform.position);

                if (distance < nearestEnemyDistance)
                {
                    nearestEnemy = gameManager.allSpawnedEnemies[i].transform;

                    nearestEnemyDistance = distance;
                }
            }


            LookAtNearstEnemy(nearestEnemy);
            return nearestEnemy;
        }

        else return null;

    }

    #endregion

    #region Firing
    private void ResetAttackSpeed()
    {
        currentAttackSpeed = playerStats.GetAttackSpeed();
    }
    #endregion

}
