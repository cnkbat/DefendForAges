using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    PlayerStats playerStats;
    GameManager gameManager;

    [SerializeField] List<Transform> enemySpawnPoses;

    [Header("Waves")]
    public List<EnemySpawner> waveList;

    [Header("Buyable Areas")]
    [SerializeField] private List<BuyableArea> areas;
    [SerializeField] private List<int> costs;

    [Header("Save & Load")]
    private int buyedAreaIndex;
    private int towerUpgradeIndex;

    [Header("Points")]
    [SerializeField] private Transform revivePoint;
    [SerializeField] private Transform startPoint;

    [Header("Targeting")]
    [SerializeField] List<EnemyTarget> targetList;

    [Header("Tower")]
    [SerializeField] private TowerBehaviour tower;

    [Header("Events")]
    public Action OnEnemySpawnPosesUpdated;
    public Action OnTargetListUpdated;
    public Action OnWaveCalled;
    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].OnWaveCompleted += StopWaves;
            waveList[i].OnWaveCompleted += playerStats.WaveWon;
        }
    }

    private void OnDisable()
    {
        playerStats = PlayerStats.instance;
        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].OnWaveCompleted -= StopWaves;
            waveList[i].OnWaveCompleted += playerStats.WaveWon;
        }
    }

    public void Start()
    {
        UpdateTargetList();
        gameManager = GameManager.instance;
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
        targetList.Clear();
        List<EnemyTarget> tempList = FindObjectsOfType<EnemyTarget>().ToList();
        
        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].GetIsTargetable())
            {
                targetList.Add(tempList[i]);
            }
        }

        OnTargetListUpdated?.Invoke();
    }

    public void WaveCalled()
    {

        StopWaves();

        EnemySpawner currentWave = waveList[playerStats.GetWaveIndex()];
        currentWave.gameObject.SetActive(true);
        gameManager.SetActiveWave(currentWave);
        OnWaveCalled?.Invoke();
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
    public List<EnemyTarget> GetTargetList()
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
