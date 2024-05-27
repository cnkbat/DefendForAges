using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    PlayerStats playerStats;

    [Header("Enemies")]
    public List<GameObject> allSpawnedEnemies;
    private EnemySpawner activeWave;
    public bool canSpawnEnemy;

    [Header("Drops")]
    [SerializeField] public List<CurrencyAnimationHandler> droppedCurrencies = new List<CurrencyAnimationHandler>();
    [SerializeField] private float droppedCollectionAnimDur;

    [Header("Phases")]
    public bool isAttackPhase;

    [Header("Cities")]
    [SerializeField] public List<CityManager> allCities;
    [SerializeField] private List<TowerBehaviour> towers;

    [Header("AI Manipulation")]
    [SerializeField] public int surfaceAreaIndex;

    [Header("**------ SETTINGS ------**")]

    [Header("Debugging")]
    [SerializeField] private int targetFPS;

    [Header("Optimization")]
    [SerializeField] public float bulletFireRange = 150f;

    [Header("*---Design & Balance ---*")]

    [Header("Defence Related")]
    [SerializeField] public float repairTimer;

    [Header("Enemy Related")]
    [SerializeField] public float enemySlowedSpeed;
    [SerializeField] public int dropTypeCount = 3;
    [SerializeField] public float enemyHealthBarDisappearTimer = 1;

    [Header("Flaoting Text Related")]
    [Tooltip("Hasara göre çarpan olarak çalışıyor.")][SerializeField] public float fontSizeOnEnemyHit = 1;
    [Tooltip("Heale göre çarpan olarak çalışıyor.")][SerializeField] public float fontSizeOnPlayerHeal = 1;

    [Header("Events")]
    public Action OnCheckPointReached;
    public Action OnEraChanged;
    public Action OnWaveStarted;
    public Action OnCityDidnotChanged;
    public Action OnApplyEarnings;

    //****** Code Cleaners *****///
    bool isEraCompleted = false;
    [HideInInspector] public int totalWaveCount;

    private void OnEnable()
    {
        Time.timeScale = 1f;
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
            totalWaveCount += allCities[i].waveList.Count;
        }

        playerStats.OnWaveWon += CheckIfEraFinished;
        OnCheckPointReached += playerStats.CityChangerReached;

        OnApplyEarnings += PlayDroppedEarningsCollectionAnim;
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
            totalWaveCount -= allCities[i].waveList.Count;
        }

        playerStats.OnWaveWon -= CheckIfEraFinished;
        OnCheckPointReached -= playerStats.CityChangerReached;

        OnApplyEarnings -= PlayDroppedEarningsCollectionAnim;

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
        playerStats.SetWaveSystemBackToCheckpoint();
    }

    public void LevelWon()
    {
        // earnings vs buraya koyulacak
        // fonksiyonun olması şart değil 
        // polish kısmında tekrardan düzenlecek.
    }

    public void CheckIfEraFinished()
    {

        if (playerStats.GetWaveIndex() >= totalWaveCount)
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

        if (playerStats.GetWaveIndex() >= allCities[playerStats.GetCityIndex()].GetCityChangingIndex())
        {
            OnCheckPointReached?.Invoke();

            for (int i = 0; i < allCities.Count; i++)
            {
                allCities[i].gameObject.SetActive(false);
            }


            allCities[playerStats.GetCityIndex()].gameObject.SetActive(true);

            return;
        }
        else
        {
            OnCityDidnotChanged?.Invoke();
        }
    }

    #endregion

    #region Wave Control

    public void OnWaveCalled()
    {
        allCities[playerStats.GetCityIndex()].WaveCalled();
        canSpawnEnemy = true;
        isAttackPhase = true;
        OnWaveStarted?.Invoke();
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
    #endregion

    #region Earnings
    public void PlayDroppedEarningsCollectionAnim()
    {
        for (int i = 0; i < droppedCurrencies.Count; i++)
        {
            float animDur = Vector3.Distance(droppedCurrencies[i].transform.position, playerStats.transform.position) / droppedCollectionAnimDur;

            droppedCurrencies[i].PlayCollectionAnim();
            droppedCurrencies[i].transform.DOMove(playerStats.transform.position, animDur).OnComplete(() => droppedCurrencies[i].gameObject.SetActive(false));
        }

        droppedCurrencies.Clear();
    }

    #endregion

    #region  Getters & Setters
    public void SetActiveWave(EnemySpawner newActiveWave)
    {
        activeWave = newActiveWave;
        activeWave.OnWaveCompleted += OnWaveFinished;
    }

    #endregion

}
