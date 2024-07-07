using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    PlayerStats playerStats;
    AsyncLoader asyncLoader;

    [Header("Enemies")]
    public List<EnemyDeathHandler> allSpawnedEnemies;
    private EnemySpawner activeWave;
    public bool canSpawnEnemy;

    [Header("Drops")]
    [SerializeField] public List<CurrencyAnimationHandler> droppedCurrencies = new List<CurrencyAnimationHandler>();
    [SerializeField] private Ease droppedAnimEaseType = Ease.Linear;
    [SerializeField] private float droppedCollectionAnimDur;

    [Header("Cities")]
    [SerializeField] public List<CityManager> allCities;

    [Header("**------ SETTINGS ------**")]

    [Header("Debugging")]
    [SerializeField] private int targetFPS;

    [Header("Optimization")]
    [SerializeField] public float bulletFireRange = 150f;

    [Header("*---Design & Balance ---*")]

    [Header("Game Management")]
    [SerializeField] public bool isPlayerFreezed;
    public bool isAttackPhase;
    public bool isGameFreezed;

    [Header("Cam Management")]
    [SerializeField] public float panCamBackToPlayerDelay = 2;

    [Header("Defence Related")]
    [SerializeField] public float repairTimer;
    [SerializeField] public int reviveTowerCost = 10;

    [Header("Enemy Related")]
    [SerializeField] public float enemySlowedSpeed;
    [SerializeField] public int dropTypeCount = 3;
    [SerializeField] public float healthBarDisappearTimer = 1;
    [SerializeField] public float bossVFXDisappearTimer = 3;

    [Header("Fading")]
    [SerializeField] public float maxTransparentTimer = 1.25f;
    [SerializeField] public float fadedObjectAlphaValue = 0.2f;
    [SerializeField] public float fadingSpeed = 5f;

    [Header("Flaoting Text Related")]
    [Tooltip("Hasara göre çarpan olarak çalışıyor.")][SerializeField] public float fontSizeOnEnemyHit = 1;

    [Header("Nav Mesh Surface")]
    private NavMeshSurface navMeshSurface;
    private List<EnemySpawner> allWaves = new List<EnemySpawner>();
    int navMeshsurfaceCounter;


    [Header("Events")]
    public Action OnCityChange;
    public Action OnEraChanged;
    public Action OnWaveStarted;
    public Action OnCityDidnotChanged;
    public Action OnApplyEarnings;
    public Action<CinemachineVirtualCamera> OnCameraPan;

    //****** Code Cleaners *****///
    bool isEraCompleted = false;
    [HideInInspector] public int totalWaveCount;

    private void OnEnable()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = targetFPS;

        playerStats = PlayerStats.instance;

        navMeshSurface = FindObjectOfType<NavMeshSurface>();

        for (int i = 0; i < allCities.Count; i++)
        {
            totalWaveCount += allCities[i].waveList.Count;

            allCities[i].GetTower().OnTargetDestroyed += FreezeGame;

            for (int j = 0; j < allCities[i].waveList.Count; j++)
            {
                allCities[i].waveList[j].OnEnemySpawned += ReAdjustNavMeshSurface;

                allWaves.Add(allCities[i].waveList[j]);
            }
        }

        playerStats.OnWaveWon += CheckIfEraFinished;
        OnCityChange += playerStats.CityChangerReached;

        OnApplyEarnings += PlayDroppedEarningsCollectionAnim;

        OnCameraPan += PanCamToObject;
    }

    private void OnDisable()
    {
        for (int i = 0; i < allCities.Count; i++)
        {
            totalWaveCount -= allCities[i].waveList.Count;

            for (int j = 0; j < allCities[i].waveList.Count; j++)
            {
                allCities[i].waveList[j].OnEnemySpawned -= ReAdjustNavMeshSurface;
            }
        }

        playerStats.OnWaveWon -= CheckIfEraFinished;
        OnCityChange -= playerStats.CityChangerReached;

        OnApplyEarnings -= PlayDroppedEarningsCollectionAnim;

        OnCameraPan -= PanCamToObject;
    }

    private void Start()
    {
        CheckIfEraFinished();

        asyncLoader = AsyncLoader.instance;
    }

    public void FreezeGame()
    {
        isGameFreezed = true;
    }

    public void ReAdjustNavMeshSurface(int enemyGroupCount)
    {
        navMeshsurfaceCounter++;
        if (navMeshsurfaceCounter >= enemyGroupCount)
        {
            MoveSurface();
            navMeshsurfaceCounter = 0;
        }
    }


    private void MoveSurface()
    {
        navMeshSurface.transform.position = new Vector3(navMeshSurface.transform.position.x,
                    Random.Range(9.80f, 10f),
                     navMeshSurface.transform.position.z);
    }

    #region  Win & Lose Conditions

    public void LevelLost()
    {
        playerStats.SetWaveSystemBackToCheckpoint();

        // oynanacak vfxler animasyonlar vs vs.

        asyncLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

            allCities[playerStats.GetCityIndex()].OnCityWon?.Invoke();

            OnCityChange?.Invoke();

            playerStats.transform.position = allCities[playerStats.GetCityIndex()].GetStartPoint();
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
        isAttackPhase = true;
        allCities[playerStats.GetCityIndex()].WaveCalled();
        canSpawnEnemy = true;
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
            Debug.Log("dropped anim");
            GameObject animatedObject = droppedCurrencies[i].gameObject;
            float animDur = Vector3.Distance(animatedObject.transform.position, playerStats.transform.position) / droppedCollectionAnimDur;
            droppedCurrencies[i].PlayCollectionAnim();

            animatedObject.transform.DOMove(playerStats.transform.position, animDur).SetEase(droppedAnimEaseType).
                OnComplete(() => animatedObject.gameObject.SetActive(false));
        }

        droppedCurrencies.Clear();
    }

    #endregion

    #region Camera Management

    public void PanCamToObject(CinemachineVirtualCamera newCam)
    {
        newCam.m_Priority = 20;

        isPlayerFreezed = true;
        isGameFreezed = true;

        StartCoroutine(PanCamBackToPlayer(newCam));
    }

    IEnumerator PanCamBackToPlayer(CinemachineVirtualCamera newCam)
    {
        yield return new WaitForSeconds(panCamBackToPlayerDelay);

        newCam.m_Priority = 0;

        isPlayerFreezed = false;
        isGameFreezed = false;
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
