using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    NearestEnemyFinder nearestEnemyFinder;

    PlayerStats playerStats;
    GameManager gameManager;

    [Header("Fire Range")]
    public float fireRange;

    [Tooltip("Attack Speed")]
    float currentAttackSpeed;

    [Tooltip("Events")]
    public Action<Transform> OnAttack;

    private void Start()
    {
        gameManager = GameManager.instance;
        playerStats = GetComponent<PlayerStats>();
        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();

        ResetAttackSpeed();

        nearestEnemyFinder.SetFireRange(fireRange);
    }

    private void Update()
    {
        if (nearestEnemyFinder.GetNearestEnemy())
        {
            LookAtNearstEnemy(nearestEnemyFinder.GetNearestEnemy());

            currentAttackSpeed -= Time.deltaTime;
            if (currentAttackSpeed <= 0)
            {
                OnAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy());
                ResetAttackSpeed();
            }
        }
        else
        {
            ResetAttackSpeed();
        }
    }

    #region  findingClosestEnemy

    private void LookAtNearstEnemy(Transform closestEnemy)
    {

        if (closestEnemy != null)
        {
            //[SerializeField] Transform headAimObject;
            // ! anim rigging yapcaz ? belki
            //  headAimObject.transform.parent = closestEnemy;
            //headAimObject.transform.localPosition = Vector3.zero;


            Vector3 worldAimTarget = closestEnemy.transform.position;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = aimDirection;

        }

    }


    #endregion

    #region Firing
    private void ResetAttackSpeed()
    {
        currentAttackSpeed = playerStats.GetAttackSpeed();
    }
    #endregion

}
