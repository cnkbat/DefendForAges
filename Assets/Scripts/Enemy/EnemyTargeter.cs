using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour, IPoolableObject
{
    [SerializeField] private Transform target;
    private float targetTimer;
    private float closestDistance;
    [SerializeField] EnemyTarget[] targetList;
    public CityManager cityManager;

    public void OnObjectPooled()
    {
        throw new System.NotImplementedException();
        // object poolera geçince start taşınacak.
    }
    private void OnEnable()
    {
        cityManager.OnTargetListUpdated += UpdateTargetList;
    }
    private void Start()
    {
        targetTimer = 2f;
        closestDistance = 999999;
    }

    private void Update()
    {
        if (targetTimer >= 2)
        {
            closestDistance = 999999;
            foreach (var targetable in targetList)
            {
                if (closestDistance > Vector3.Distance(transform.position, targetable.GetTarget().position))
                {
                    closestDistance = Vector3.Distance(transform.position, targetable.GetTarget().position);
                    SetTarget(targetable.GetTarget());
                    targetTimer = 0;
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
        targetList = cityManager.GetTargetList();
    }

    #region  Getters & Setters
    public void SetCityManager(CityManager newCityManager)
    {
        cityManager = newCityManager;
    }
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
