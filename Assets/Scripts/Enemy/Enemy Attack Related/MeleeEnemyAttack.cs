using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : EnemyAttack
{
    public void DealMeleeDamage()
    {
      
        enemyTargeter.GetTargetedObject().TakeDamage(enemyStats.GetDamage());
    }
}
