using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : DefencesBehaviourBase
{

    //override protected void Start()
    //{
    //    base.Start();
    //    wallStats = GetComponent<WallStats>();
    //}
    public override void TakeDamage(float dmg)
    {
        WallStats wallStats = GetComponent<WallStats>();
        currentHealth -= dmg;
        if (currentHealth < 12)
        {
            if (currentHealth < 10)
            {
                if (currentHealth < 8)
                {
                    if (currentHealth < 6)
                    {
                        if (currentHealth <= 0)
                        {
                            DestroyDefence();
                        }
                        wallStats.wallParts[3].SetActive(false);
                    }
                    wallStats.wallParts[2].SetActive(false);
                }
                wallStats.wallParts[1].SetActive(false);
            }
            wallStats.wallParts[0].SetActive(false);
        }


    }
}
