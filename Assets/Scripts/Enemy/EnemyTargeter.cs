using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    private Transform target;
    private float targetTimer;
    private float closestDistance;
    
    private void Start()
    {
        targetTimer = 2f;
        closestDistance = -1;
    }

    // every two seconds, find the closest target
    private void Update()
    {
        if(targetTimer >= 2)
        {
            var targetList = FindObjectsOfType<MonoBehaviour>().OfType<ITargetable>();
            
            foreach(var targetable in targetList)
            {
                if(closestDistance == -1)
                {
                    SetTarget(targetable.target());
                }
                else
                {
                    if(closestDistance > Vector3.Distance(transform.position, targetable.target().position))
                    {
                        closestDistance = Vector3.Distance(transform.position, targetable.target().position);
                        SetTarget(targetable.target());
                    }
                }
            }
        }
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget()
    {
        return target;
    }
}
