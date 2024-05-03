using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float targetTimer;
    private float closestDistance;

    [Header("City Related")]
    [SerializeField] EnemyTarget[] targetList;
    public CityManager cityManager;
    
    public void EnemySpawned()
    {
        UpdateTargetList();
        targetTimer = 2f;
        closestDistance = 999999;
    }

    private void OnEnable()
    {
        if (cityManager != null)
        {
            cityManager.OnTargetListUpdated += UpdateTargetList;
        }
    }

    private void OnDisable()
    {
        if (cityManager != null)
        {
            cityManager.OnTargetListUpdated -= UpdateTargetList;
        }
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
