using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("Enemies")]
    public List<GameObject> allSpawnedEnemies;
    public EnemySpawner activeWave;
    public bool canSpawnEnemy;

    [Header("Cities")]
    [SerializeField] private List<CityManager> allCities;
    [SerializeField] private int currentCityIndex;
    [SerializeField] private List<TowerBehaviour> towers;

    void Start()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].OnTowerDestroyed += LevelLost;
        }
    }

    public void LevelLost()
    {
        // use gem to recover
        // #if gem <0 gem offer
        // revive option
        // yoksa her şey level bitti.
    }

    public void SetActiveWave(EnemySpawner newActiveWave)
    {
        activeWave = newActiveWave;
    }

    #region  Getters & Setters
    public void SetCurrentCityIndex(int newIndex)
    {
        currentCityIndex = newIndex;
    }

    #endregion

}
