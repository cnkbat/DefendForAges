using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestEnemyFinder : MonoBehaviour
{
    GameManager gameManager;

    [Header("Nearest Enemy Finding")]
    private float nearestEnemyDistance;
    float distance;
    private Transform nearestEnemy;
    private float fireRange;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    #region Finding Nearest Enemy

    public Transform GetNearestEnemy()
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


            if (nearestEnemyDistance < fireRange)
            {
                return nearestEnemy;
            }
            else return null;
        }
        else return null;
    }

    #endregion


    #region Getters & Setters

    public void SetFireRange(float value)
    {
        fireRange = value;
    }
    #endregion
}
