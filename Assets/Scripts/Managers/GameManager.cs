using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    PlayerStats playerStats;

    [Header("Enemies")]
    public List<GameObject> allSpawnedEnemies;
    public EnemySpawner activeWave;
    public bool canSpawnEnemy;

    [Header("Cities")]
    [SerializeField] public List<CityManager> allCities;
    [SerializeField] private List<TowerBehaviour> towers;

    [Header("Events")]
    public Action OnCheckPointReached;

    //******///
    bool isEraCompleted = false;
    int allWavesCount;

    void Start()
    {
        playerStats = PlayerStats.instance;

        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].OnTowerDestroyed += LevelLost;
        }

        for (int i = 0; i < allCities.Count; i++)
        {
            allWavesCount += allCities[i].waveList.Count;
        }

        playerStats.OnWaveWon += CheckIfCheckPointReached;

        OnCheckPointReached += playerStats.CheckPointReached;
    }

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
        if (playerStats.GetCurrentWaveIndex() >= allWavesCount)
        {
            isEraCompleted = true;
            // animasyon oynaması
            // ui update
            // era bitişiyle ilgili durumlar

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

        if (playerStats.GetCurrentWaveIndex() >= allCities[playerStats.GetCurrentCityIndex()].waveList.Count)
        {
            OnCheckPointReached?.Invoke();
            // şehirdeki animasyonlar
            // Fxler
        }
    }
    public void SetActiveWave(EnemySpawner newActiveWave)
    {
        activeWave = newActiveWave;
    }

    #region  Getters & Setters

    #endregion

}
