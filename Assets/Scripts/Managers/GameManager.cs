using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    PlayerStats playerStats;
    AsyncLoader asyncLoader;

    [Header("Enemies")]
    public List<GameObject> allSpawnedEnemies;
    private EnemySpawner activeWave;
    public bool canSpawnEnemy;

    [Header("Drops")]
    [SerializeField] public List<CurrencyAnimationHandler> droppedCurrencies = new List<CurrencyAnimationHandler>();
    [SerializeField] private float droppedCollectionAnimDur;

    [Header("Cities")]
    [SerializeField] public List<CityManager> allCities;

    [Header("AI Manipulation")]
    [SerializeField] public int surfaceAreaIndex;
    [SerializeField] public float samplePositionRadius;

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

    [Header("Game End Related")]
    [SerializeField] private float gameLoseDelayAfterButtonPressed = 3;


    [Header("Events")]
    public Action OnCheckPointReached;
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

        for (int i = 0; i < allCities.Count; i++)
        {
            totalWaveCount += allCities[i].waveList.Count;
            allCities[i].GetTower().OnTargetDestroyed += FreezeGame;
        }

      
        playerStats.OnWaveWon += CheckIfEraFinished;
        OnCheckPointReached += playerStats.CityChangerReached;

        OnApplyEarnings += PlayDroppedEarningsCollectionAnim;

        OnCameraPan += PanCamToObject;
    }

    private void OnDisable()
    {
        for (int i = 0; i < allCities.Count; i++)
        {
            totalWaveCount -= allCities[i].waveList.Count;
        }

        playerStats.OnWaveWon -= CheckIfEraFinished;
        OnCheckPointReached -= playerStats.CityChangerReached;

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
    
    #region  Win & Lose Conditions

    public void LevelLost()
    {
        playerStats.SetWaveSystemBackToCheckpoint();

        // oynanacak vfxler animasyonlar vs vs.

        StartCoroutine(LevelLostDelayFunc());
    }
    IEnumerator LevelLostDelayFunc()
    {
        yield return new WaitForSeconds(gameLoseDelayAfterButtonPressed);

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
            GameObject animatedObject = droppedCurrencies[i].gameObject;
            float animDur = Vector3.Distance(animatedObject.transform.position, playerStats.transform.position) / droppedCollectionAnimDur;
            droppedCurrencies[i].PlayCollectionAnim();

            animatedObject.transform.DOMove(playerStats.transform.position, animDur).
                OnComplete(() => animatedObject.SetActive(false));
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
