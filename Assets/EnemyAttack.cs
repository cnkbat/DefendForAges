using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ITargetable targetable))
        {
            this.transform.root.GetComponent<EnemyBehaviour>().Attack(targetable);
        }
    }
}
