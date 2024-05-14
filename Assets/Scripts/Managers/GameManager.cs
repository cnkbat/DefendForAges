using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    PlayerStats playerStats;

    [Header("Enemies")]
    public List<GameObject> allSpawnedEnemies;
    private EnemySpawner activeWave;
    public bool canSpawnEnemy;

    [Header("Phases")]
    public bool isAttackPhase;

    [Header("Cities")]
    [SerializeField] public List<CityManager> allCities;
    [SerializeField] private List<TowerBehaviour> towers;

    [Header("Debugging")]
    [SerializeField] private int targetFPS;

    [Header("Optimization")]
    [SerializeField] public float bulletFireRange = 150f;

    [Header("Events")]
    public Action OnCheckPointReached;
    public Action OnEraChanged;

    //******///
    bool isEraCompleted = false;
    int allWavesCount;

    private void OnEnable()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = targetFPS;

        for (int i = 0; i < allCities.Count; i++)
        {
            towers.Add(allCities[i].GetTower());
        }

        playerStats = PlayerStats.instance;

        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].OnTowerDestroyed += LevelLost;
        }

        for (int i = 0; i < allCities.Count; i++)
        {
            allWavesCount += allCities[i].waveList.Count;
        }

        playerStats.OnWaveWon += CheckIfEraFinished;

        OnCheckPointReached += playerStats.CheckPointReached;
    }

    private void OnDisable()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].OnTowerDestroyed -= LevelLost;
        }

        towers.Clear();

        for (int i = 0; i < allCities.Count; i++)
        {
            allWavesCount -= allCities[i].waveList.Count;
        }

        playerStats.OnWaveWon -= CheckIfEraFinished;

        OnCheckPointReached -= playerStats.CheckPointReached;
    }
    private void Start()
    {
        CheckIfEraFinished();
    }
    #region  Win & Lose Conditions

    public void LevelLost()
    {
        // use gem to recover
        // #if gem <0 gem offer
        // revive option
        // yoksa her şey level bitti.
    }

    public void LevelWon()
    {
        // earnings vs buraya koyulacak
        // fonksiyonun olması şart değil 
        // polish kısmında tekrardan düzenlecek.
    }

    public void CheckIfEraFinished()
    {

        if (playerStats.GetWaveIndex() >= allWavesCount)
        {
            isEraCompleted = true;
            // animasyon oynaması
            // ui update
            // era bitişiyle ilgili durumlar
            OnEraChanged?.Invoke();

            LevelWon();
        }
        else
        {
            CheckIfCheckPointReached();
        }
    }

    public void CheckIfCheckPointReached()
    {
        if (isEraCompleted) return;

        if (playerStats.GetWaveIndex() >= allCities[playerStats.GetCityIndex()].checkpointReachedIndexer)
        {
            OnCheckPointReached?.Invoke();
        }

        Debug.Log("city index " + playerStats.GetCityIndex());

        for (int i = 0; i < allCities.Count; i++)
        {
            allCities[i].gameObject.SetActive(false);
        }


        allCities[playerStats.GetCityIndex()].gameObject.SetActive(true);
    }

    #endregion

    public void OnWaveCalled()
    {
        allCities[playerStats.GetCityIndex()].WaveCalled();
        canSpawnEnemy = true;
        isAttackPhase = true;
    }

    public void OnWaveFinished()
    {

        for (int i = 0; i < allSpawnedEnemies.Count; i++)
        {
            if (allSpawnedEnemies[i].TryGetComponent(out IPoolableObject poolableObject))
            {
                poolableObject.ResetObjectData();
            }
        }

        isAttackPhase = false;
        allSpawnedEnemies.Clear();
    }

    #region  Getters & Setters
    public void SetActiveWave(EnemySpawner newActiveWave)
    {
        activeWave = newActiveWave;
        activeWave.OnWaveCompleted += OnWaveFinished;
    }

    #endregion

}
