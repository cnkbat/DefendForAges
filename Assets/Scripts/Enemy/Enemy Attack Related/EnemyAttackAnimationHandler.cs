using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnimationHandler : MonoBehaviour
{
    public BoxCollider rightHandCollider;
    public EnemyBehaviour enemyBehaviour;
    public void Start()
    {
        enemyBehaviour = transform.GetComponentInParent<EnemyBehaviour>();
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
