using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestEnemyFinder : MonoBehaviour
{
    GameManager gameManager;

    [Header("Nearest Enemy Finding")]
    [SerializeField] private float lookAtRange;
    private float nearestEnemyDistance;
    float distance;
    private EnemyDeathHandler nearestEnemy;
    private float fireRange;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    #region Finding Nearest Enemy

    public EnemyDeathHandler GetNearestEnemy()
    {

        nearestEnemyDistance = 10000f;

        if (gameManager.allSpawnedEnemies.Count > 0)
        {

            for (int i = 0; i < gameManager.allSpawnedEnemies.Count; i++)
            {
                distance = Vector3.Distance(transform.position, gameManager.allSpawnedEnemies[i].transform.position);

                if (distance < nearestEnemyDistance)
                {
                    nearestEnemy = gameManager.allSpawnedEnemies[i];

                    nearestEnemyDistance = distance;
                }
            }

            if (nearestEnemyDistance < fireRange + lookAtRange)
            {
                return nearestEnemy;
            }
            else return null;
        }
        else return null;
    }

    #endregion


    public bool GetIsEnemyInFiringRange()
    {
        if (nearestEnemyDistance < fireRange)
        {
            return true;
        }
        else return false;
    }

    #region Getters & Setters

    public void SetFireRange(float value)
    {
        fireRange = value;
    }

    #endregion

}
