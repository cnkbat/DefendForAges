using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyBehaviour enemyBehaviour;

    private void Start()
    {
        enemyBehaviour = transform.GetComponentInParent<EnemyBehaviour>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ITargetable targetable))
        {
           // enemyBehaviour.DealDamage(targetable);
        }
    }
}
