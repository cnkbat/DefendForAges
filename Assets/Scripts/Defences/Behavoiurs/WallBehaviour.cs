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
            wallStats.wallParts[0].SetActive(false);
        }
        if (currentHealth < 10)
        {
            wallStats.wallParts[1].SetActive(false);
        }
        if (currentHealth < 8)
        {
            wallStats.wallParts[2].SetActive(false);
        }
        if (currentHealth < 6)
        {
            wallStats.wallParts[3].SetActive(false);
        }
        if (currentHealth <= 0)
        {
            DestroyDefence();
        }
    }
}
