using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    PlayerStats playerStats;

    [SerializeField] List<Transform> enemySpawnPoses;

    [Header("Waves")]
    public List<EnemySpawner> waveList;

    [Header("Points")]
    [SerializeField] private Transform revivePoint;
    [SerializeField] private Transform startPoint;

    [Header("Targeting")]
    [SerializeField] EnemyTarget[] targetList;

    [Header("Tower")]
    [SerializeField] private TowerBehaviour tower;

    [Header("Events")]
    public Action OnEnemySpawnPosesUpdated;
    public Action OnTargetListUpdated;


    public void Start()
    {
        UpdateTargetList();

        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].OnWaveCompleted += StopWaves;
        }
        playerStats = PlayerStats.instance;
    }

    public void AddEnemyPos(Transform newTransform)
    {
        enemySpawnPoses.Add(newTransform);
        OnEnemySpawnPosesUpdated?.Invoke();
    }

    // this function has to be called everytime when a new targetable is spawned or when a targetable is destroyed.
    public void UpdateTargetList()
    {
        targetList = FindObjectsOfType<EnemyTarget>();
        OnTargetListUpdated?.Invoke();
    }

    public void OnWaveCalled()
    {
        StopWaves();

        waveList[playerStats.GetCurrentWaveIndex()].gameObject.SetActive(true);
    }

    public void StopWaves()
    {
        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].gameObject.SetActive(false);
        }
    }

    #region Getters & Setters
    public TowerBehaviour GetTower()
    {
        return tower;
    }
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
