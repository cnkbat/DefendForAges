using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float targetTimer = 3;
    private float currentTargetTimer;
    private float closestDistance;

    [Header("City Related")]
    public CityManager cityManager;

    public void EnemySpawned()
    {
        ResetTargetTimer();
        closestDistance = 999999;
    }

    private void OnEnable()
    {
        cityManager = FindObjectOfType<CityManager>();

        cityManager.OnTargetListUpdated += ResetTargetTimer;
    }

    private void OnDisable()
    {
        cityManager.OnTargetListUpdated -= ResetTargetTimer;
    }

    private void Update()
    {
        currentTargetTimer -= Time.deltaTime;
        if (currentTargetTimer < 0)
        {
            closestDistance = 999999;
            foreach (var targetable in cityManager.GetTargetList())
            {
                if (closestDistance > Vector3.Distance(transform.position, targetable.GetTarget().position))
                {
                    closestDistance = Vector3.Distance(transform.position, targetable.GetTarget().position);
                    SetTarget(targetable.GetTarget());
                    UpdateTargetTimer();
                }
            }
        }
    }

    private void UpdateTargetTimer()
    {
        currentTargetTimer = targetTimer;
    }

    private void ResetTargetTimer()
    {
        currentTargetTimer = 0;
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
