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
    int waveIndex;
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
        playerStats.currentWaveIndex = waveIndex;
        OnNewTarget += UpdateTargetList;
        OnRemoveTarget += UpdateTargetList;
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
        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].gameObject.SetActive(false);
        }

        waveList[waveIndex].gameObject.SetActive(true);
    }


    // waveler tamamen bittiğinde çalışacak.    
    public void IncrementWaveIndex()
    {
        waveIndex++;
    }

    public void SetWaveSystemBackToCheckpoint()
    {
        // gamemanagerdan eventle ulaşılması
        // wave indexin en son checkpoint değerine atanması
        // save edilmesi
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
