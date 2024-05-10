using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityManager : MonoBehaviour
{
    PlayerStats playerStats;
    GameManager gameManager;
    SaveManager saveManager;

    [SerializeField] List<Transform> enemySpawnPoses;

    [Header("Waves")]
    public List<EnemySpawner> waveList;

    [Header("Buyable Areas")]
    [SerializeField] public List<BuyableArea> buyableAreas;
    [SerializeField] public List<int> buyableAreaCosts;


    [Header("Save & Load")]
    public string cityName;
    public int buyedAreaIndex;
    public int towerUpgradeIndex;


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
        saveManager = SaveManager.instance;
        LoadCityManagerData();

        saveManager.OnSaved += SaveCityManagerData;
        saveManager.OnResetData += DeleteCityManagerData;

        playerStats = PlayerStats.instance;
        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].OnWaveCompleted += StopWaves;
            waveList[i].OnWaveCompleted += playerStats.WaveWon;
        }

    }

    private void OnDisable()
    {
        saveManager.OnSaved -= SaveCityManagerData;
        saveManager.OnResetData -= DeleteCityManagerData;

        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].OnWaveCompleted -= StopWaves;
            waveList[i].OnWaveCompleted -= playerStats.WaveWon;
        }
    }

    public void Start()
    {
        UpdateTargetList();
        gameManager = GameManager.instance;
        playerStats = PlayerStats.instance;

        for (int i = 0; i < buyedAreaIndex; i++)
        {
            buyableAreas[i].EnableArea();
        }

    }

    #region Area Buying
    public void AreaBuyed()
    {
        Debug.Log("areabuyed");

        buyedAreaIndex += 1;

        saveManager.OnSaved?.Invoke();
    }

    public void AreaEnabled(List<Transform> addedPoses)
    {
        for (int i = 0; i < addedPoses.Count; i++)
        {
            AddEnemyPos(addedPoses[i]);
        }
    }
    #endregion

    #region Enemy Related
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
    #endregion

    #region Save & Load

    public void SaveCityManagerData()
    {
        SaveSystem.SaveCityManagerData(this, cityName);
    }

    public void LoadCityManagerData()
    {
        CityManagerData cityManagerData = SaveSystem.LoadCityManagerData(cityName);

        if (cityManagerData != null)
        {
            this.buyedAreaIndex = cityManagerData.buyedAreaIndex;
            this.towerUpgradeIndex = cityManagerData.towerUpgradeIndex;
        }
    }

    public void DeleteCityManagerData()
    {
        SaveSystem.DeleteCityManagerData(cityName);
    }

    #endregion

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
