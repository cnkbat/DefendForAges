using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : DefencesBehaviourBase
{
    public override void TakeDamage(float dmg)
    {
        // start fonksiyonu kullanırsam superclassın startını overridelıyor, wallstatsı burada her hasar aldığında alıyorum, değiştirilmeli
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
