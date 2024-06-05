using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRayCast : MonoBehaviour
{
    PlayerStats playerStats;
    GameManager gameManager;

    private void Start()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;
    }

    void Update()
    {
        Vector3 playerRayDir = playerStats.transform.position - transform.position;

        Ray playerRay = new Ray(transform.position, playerRayDir);
        RaycastHit playerRayHit;

        if (Physics.Raycast(playerRay, out playerRayHit))
        {
            if (playerRayHit.collider.TryGetComponent(out IFadeable fadeable))
            {
                fadeable.SetFade(true);
            }
        }


        for (int i = 0; i < gameManager.allSpawnedEnemies.Count; i++)
        {
            Vector3 enemyRaydir = gameManager.allSpawnedEnemies[i].transform.position - transform.position;

            Ray enemyRay = new Ray(transform.position, enemyRaydir);
            RaycastHit enemyRayHit;

            if (Physics.Raycast(enemyRay, out enemyRayHit))
            {
                if (enemyRayHit.collider.TryGetComponent(out IFadeable fadeable))
                {
                    fadeable.SetFade(true);
                }
            }
        }

    }

}
