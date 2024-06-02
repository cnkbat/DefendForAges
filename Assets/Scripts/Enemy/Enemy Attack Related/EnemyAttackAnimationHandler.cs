using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnimationHandler : MonoBehaviour
{
    EnemyAttack enemyAttack;
    private BoxCollider rightHandCollider;
    private EnemyTargeter enemyTargeter;
    private EnemyStats enemyStats;

    public void Start()
    {
        enemyStats = transform.GetComponentInParent<EnemyStats>();
        enemyTargeter = transform.GetComponentInParent<EnemyTargeter>();

        enemyAttack = GetComponentInChildren<EnemyAttack>();
        rightHandCollider = enemyAttack.GetComponent<BoxCollider>();
    }
    public void EnableDamage()
    {
        enemyTargeter.GetTargetedObject().TakeDamage(enemyStats.GetDamage());
    }
    public void DisableDamage()
    {
      //  rightHandCollider.enabled = false;
    }
}
