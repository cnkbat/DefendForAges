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

        if (objectToAttack.isPlayer)
        {
            objectPooler.SpawnHitFXFromPool("HitParticle", objectToAttack.transform.position);
        }
        else
        {
            objectPooler.SpawnHitFXFromPool("HitParticle", transform.position + (transform.forward * enemyStats.GetRange()));
        }
    }

    public void SetObjectToAttack(EnemyTarget newValue)
    {
        objectToAttack = newValue;
    }
}
