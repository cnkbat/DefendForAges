using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : EnemyAttack
{
    public EnemyTarget objectToAttack;
    private void OnEnable()
    {
        OnAttackedObjectSet += SetObjectToAttack;
    }
    public void DealMeleeDamage()
    {
        if (objectToAttack == null) return;

        objectToAttack.TakeDamage(enemyStats.GetDamage());
    }

    public void SetObjectToAttack(EnemyTarget newValue)
    {
        objectToAttack = newValue;
    }
}
