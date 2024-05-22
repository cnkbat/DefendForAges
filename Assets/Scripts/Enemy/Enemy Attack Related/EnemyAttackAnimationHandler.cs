using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnimationHandler : MonoBehaviour
{
    EnemyAttack enemyAttack;
    private BoxCollider rightHandCollider;
    private EnemyBehaviour enemyBehaviour;

    public void Start()
    {
        enemyBehaviour = transform.GetComponentInParent<EnemyBehaviour>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        rightHandCollider = enemyAttack.GetComponent<BoxCollider>();
    }
    public void EnableDamage()
    {
        rightHandCollider.enabled = true;
    }
    public void DisableDamage()
    {
        rightHandCollider.enabled = false;
    }
}
