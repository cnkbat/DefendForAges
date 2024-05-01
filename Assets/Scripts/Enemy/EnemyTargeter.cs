using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour, IPoolableObject
{
    private Transform target;
    private float targetTimer;
    private float closestDistance;
    [SerializeField] EnemyTarget[] targetList;

    public void OnObjectPooled()
    {
        throw new System.NotImplementedException();
        // object poolera geçince start taşınacak.
    }
    
    private void Start()
    {
        targetTimer = 2f;
        closestDistance = -1;
        UpdateTargetList();
    }

    private void Update()
    {
        if (targetTimer >= 2)
        {
            foreach (var targetable in targetList)
            {
                if (closestDistance == -1)
                {
                    closestDistance = Vector3.Distance(transform.position, targetable.GetTarget().position);
                    SetTarget(targetable.GetTarget());
                    targetTimer = 0;
                }
                else
                {
                    if (closestDistance > Vector3.Distance(transform.position, targetable.GetTarget().position))
                    {
                        closestDistance = Vector3.Distance(transform.position, targetable.GetTarget().position);
                        SetTarget(targetable.GetTarget());
                        targetTimer = 0;
                    }
                }
            }
        }
        else
        {
            targetTimer += Time.deltaTime;
        }
    }

    private void UpdateTargetList()
    {
        targetList = FindObjectsOfType<EnemyTarget>();
    }

    #region  Getters & Setters
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget()
    {
        return target;
    }

    #endregion
}
