using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{

    [SerializeField] List<Transform> enemySpawnPoses;
    public Action OnEnemySpawnPosesUpdated;
    [SerializeField] private Transform revivePoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] EnemyTarget[] targetList;
    public void AddEnemyPos(Transform newTransform)
    {
        enemySpawnPoses.Add(newTransform);
        OnEnemySpawnPosesUpdated?.Invoke();
    }
    // this function has to be called everytime when a new targetable is spawned or when a targetable is destroyed.
    public void UpdateTargetList()
    {
        targetList = FindObjectsOfType<EnemyTarget>();
    }

    #region Getters & Setters

    public EnemyTarget[] GetTargetList()
    {
        return targetList;
    }
    public List<Transform> GetEnemySpawnPoses()
    {
        return enemySpawnPoses;
    }

    public Transform GetRevivePoint()
    {
        return revivePoint;
    }

    public Transform GetStartPoint()
    {
        return startPoint;
    }

    #endregion
}
