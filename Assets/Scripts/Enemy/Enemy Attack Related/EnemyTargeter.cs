using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTargeter : MonoBehaviour
{
    PlayerStats playerStats;

    [SerializeField] private Transform target;
    private EnemyTarget targetedObject;

    [SerializeField] private float targetTimer = 3;
    private float currentTargetTimer;
    private float closestDistance;

    [Header("City Related")]
    private CityManager cityManager;

    public Action<Transform> OnTargetChanged;

    public void EnemySpawned()
    {
        ResetTargetTimer();
        closestDistance = 999999;

        Debug.Log("enemySpawned");
        cityManager = transform.root.GetComponent<CityManager>();
        cityManager.OnTargetListUpdated += ResetTargetTimer;
        
        playerStats = PlayerStats.instance;

        if (playerStats != null)
        {
            playerStats.OnPlayerKilled += ResetTargetTimer;
        }
    }

    private void OnDisable()
    {
        if (cityManager != null)
        {
            cityManager.OnTargetListUpdated -= ResetTargetTimer;
        }

        if (playerStats != null)
        {
            playerStats.OnPlayerKilled -= ResetTargetTimer;
        }
    }

    private void Update()
    {
        if (cityManager == null)
        {
            Debug.LogWarning("city manager null");
            return;
        }

        currentTargetTimer -= Time.deltaTime;

        if (currentTargetTimer < 0)
        {
            Transform tempTargetTransform = transform;
            EnemyTarget tempEnemyTarget = null;

            closestDistance = 999999;

            foreach (var targetable in cityManager.GetTargetList())
            {
                if (closestDistance > Vector3.Distance(transform.position, targetable.GetTarget().position))
                {

                    tempEnemyTarget = targetable;
                    Transform targetableTransform = tempEnemyTarget.GetTarget();

                    closestDistance = Vector3.Distance(transform.position, targetableTransform.position);
                    tempTargetTransform = targetableTransform;

                    UpdateTargetTimer();
                }
            }

            if (targetedObject == null || targetedObject != tempEnemyTarget)
            {
                SetTarget(tempTargetTransform, tempEnemyTarget);
            }
            else
            {
                return;
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

    public void SetTarget(Transform newTarget, EnemyTarget newEnemyTarget)
    {
        targetedObject = newEnemyTarget;
        target = newTarget;

        OnTargetChanged?.Invoke(target);
    }

    public Transform GetTargetTransform()
    {
        return target;
    }

    public EnemyTarget GetTargetedObject()
    {
        return targetedObject;
    }

    public CityManager GetCityManager()
    {
        return cityManager;
    }

    #endregion
}
