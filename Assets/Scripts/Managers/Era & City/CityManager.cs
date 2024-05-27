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

    [Header("Checkpoint")]
    [Tooltip("Şehir Değiştirme Wave Indexi")][SerializeField] private int cityChangingIndex;
    [Tooltip("Kaybedince geri gittiğimiz wave indexi")][SerializeField] private int firstCheckpointIndex;
    [Tooltip("Şehirde Checkpoint Wave Indexi")][SerializeField] private int secondCheckpointIndex;

    [Header("Waves")]
    public List<EnemySpawner> waveList;
    private EnemySpawner currentWave;

    [Header("Buyable Areas")]
    [SerializeField] public List<BuyableArea> buyableAreas;
    [SerializeField] List<int> buyableAreaEnablingIndexes;
    [SerializeField] public List<int> buyableAreaCosts;

    [Header("Save & Load")]
    public string cityName;
    [HideInInspector] public int buyedAreaIndex;
    [HideInInspector] public int towerUpgradeIndex;

    [Header("Points")]
    [SerializeField] private Transform revivePoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] List<Transform> enemySpawnPoses;

    [Header("Targeting")]
    [SerializeField] List<EnemyTarget> targetList;

    [Header("Tower")]
    [SerializeField] private TowerBehaviour tower;

    [Header("Events")]
    public Action OnTargetListUpdated;
    public Action OnWaveCalled;

    #region OnEnable & OnDisable & Start
    private void OnEnable()
    {
        saveManager = SaveManager.instance;
        gameManager = GameManager.instance;

        LoadCityManagerData();

        saveManager.OnSaved += SaveCityManagerData;
        saveManager.OnResetData += DeleteCityManagerData;

        playerStats = PlayerStats.instance;

        for (int i = 0; i < waveList.Count; i++)
        {
            waveList[i].OnWaveCompleted += StopWaves;
            waveList[i].OnWaveCompleted += playerStats.WaveWon;
        }

        playerStats.OnPlayerKilled += UpdateTargetList;
        playerStats.OnPlayerRevived += UpdateTargetList;


        gameManager.OnCityDidnotChanged += CheckForSecondCheckpointReached;

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

        playerStats.OnPlayerKilled -= UpdateTargetList;
        playerStats.OnPlayerRevived -= UpdateTargetList;


        gameManager.OnCityDidnotChanged -= CheckForSecondCheckpointReached;
    }

    public void Start()
    {
        UpdateTargetList();
        HandleAreaBuyingState();
    }

    #endregion

    #region Area Buying

    public void HandleAreaBuyingState()
    {

        for (int i = 0; i < buyableAreas.Count; i++)
        {
            buyableAreas[i].DisableBuying();
        }

        for (int i = 0; i < buyableAreas.Count; i++)
        {
            if (playerStats.GetPlayerLevel() >= buyableAreaEnablingIndexes[i])
            {
                buyableAreas[i].EnableBuying();
            }
        }

        for (int i = 0; i < buyedAreaIndex; i++)
        {
            buyableAreas[i].EnableArea();
        }
    }

    public void AreaBuyed()
    {
        buyedAreaIndex += 1;

        saveManager.OnSaved?.Invoke();
    }

    public void AreaEnabled(int newSurfaceAreaIndex)
    {
        gameManager.surfaceAreaIndex = newSurfaceAreaIndex;
    }
    
    #endregion

    #region Enemy Related

    // this function has to be called everytime when a new targetable is spawned or when a targetable is destroyed.
    public void UpdateTargetList()
    {
        targetList.Clear();
        List<EnemyTarget> tempList = FindObjectsOfType<EnemyTarget>().ToList();

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].gameObject.activeSelf)
            {
                if (tempList[i].GetIsTargetable())
                {
                    targetList.Add(tempList[i]);
                }
            }
        }

        OnTargetListUpdated?.Invoke();
    }

    public void WaveCalled()
    {
        StopWaves();

        currentWave = waveList[playerStats.GetWaveIndex()];
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


    public void CheckForSecondCheckpointReached()
    {
        if (this == gameManager.allCities[playerStats.GetCityIndex()])
        {
            if (playerStats.GetWaveIndex() >= secondCheckpointIndex)
            {
                firstCheckpointIndex = secondCheckpointIndex;
            }
        }
    }


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

    public Vector3 GetStartPoint()
    {
        return startPoint.position;
    }

    public EnemySpawner GetCurrentWave()
    {
        return currentWave;
    }

    public int GetCurrentCheckpointIndex()
    {
        return firstCheckpointIndex;
    }

    public int GetCityChangingIndex()
    {
        return cityChangingIndex;
    }

    #endregion

}
