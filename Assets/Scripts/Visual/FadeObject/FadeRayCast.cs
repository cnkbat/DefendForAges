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

        if(Physics.Raycast(playerRay, out playerRayHit))
        {
            if(playerRayHit.collider.TryGetComponent(out IFadeable fadeable))
            {
                fadeable.SetFade(true);
            }
        }

        
    }

}
