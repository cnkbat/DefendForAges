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

    public void AddEnemyPos(Transform newTransform)
    {
        enemySpawnPoses.Add(newTransform);
        OnEnemySpawnPosesUpdated?.Invoke();
    }

    #region Getters & Setters
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
