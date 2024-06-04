using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyDeathHandler : MonoBehaviour, IDamagable
{
    private GameManager gameManager;
    private PlayerStats playerStats;
    private EarningsHolder earningsHolder;

    private EnemyStats enemyStats;
    private EnemyMovement enemyMovement;
    private NavMeshAgent navMeshAgent;
    private MMFeedbacks feelFeedBacks;

    [Header("States")]
    private bool isDead;

    [Header("Layers")]
    [SerializeField] private string aliveLayerName;
    [SerializeField] private string deadLayerName;

    [Header("Health")]
    private float currentHealth;

    [Header("*--- Visuals ---*")]

    [Header("Animation")]
    [SerializeField] private float deathAnimDur;

    [Header("Color Change On Death")]
    [SerializeField] private List<int> materialIndexes = new List<int>();
    private Renderer boundRenderer;
    [SerializeField] private Material originalMat;
    [SerializeField] private Material deadMat;

    [Header("Events")]
    public Action OnDamageTaken;
    public Action OnDeath;

    [Header("Visual Events")]
    public Action<int> OnDropAnimNeeded;
    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyMovement = GetComponent<EnemyMovement>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        feelFeedBacks = GetComponentInChildren<MMFeedbacks>();

        if (feelFeedBacks != null)
        {
            feelFeedBacks.Initialization();
        }
    }

    public void EnemySpawned()
    {
        earningsHolder = EarningsHolder.instance;
        gameManager = GameManager.instance;
        playerStats = PlayerStats.instance;

        isDead = false;
        boundRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        ChangeEnemyMat(originalMat);
        RefillHealth(enemyStats.GetMaxHealth());
        SetObjectLayer(aliveLayerName);

        OnDeath += enemyStats.GetAssignedEnemySpawner().OnEnemyKilled;
    }

    public void ResetData()
    {
        OnDeath -= enemyStats.GetAssignedEnemySpawner().OnEnemyKilled;
    }

    #region Kill & Take Damage

    public void TakeDamage(float dmg)
    {

        currentHealth -= dmg;

        navMeshAgent.speed = gameManager.enemySlowedSpeed;

        if (isDead)
        {
            return;
        }
        else
        {
            feelFeedBacks?.PlayFeedbacks();
        }

        if (currentHealth <= 0)
        {
            Kill();
        }
        else
        {
            OnDamageTaken?.Invoke();
        }

        StartCoroutine(enemyMovement.EnableMovement(enemyStats.GetKnockbackDuration()));

    }

    public void Kill()
    {
        isDead = true;
        //canMove = false;
        navMeshAgent.speed = 0;
        // OnMovementSpeedChanged.Invoke(0);

        ChangeEnemyMat(deadMat);

        ApplyPlayerEarnings();

        gameManager.allSpawnedEnemies.Remove(gameObject);

        PlayDropSystemAnimation();

        OnDeath?.Invoke();
        SetObjectLayer(deadLayerName);

        StartCoroutine(KillEnemy());
    }

    private void ApplyPlayerEarnings()
    {
        playerStats.OnKillEnemy?.Invoke(enemyStats.GetPowerUpValue());
        earningsHolder.OnTempEarningsUpdated?.Invoke(Mathf.RoundToInt(enemyStats.GetMoneyValue()),
            Mathf.RoundToInt(enemyStats.GetExpValue()), Mathf.RoundToInt(enemyStats.GetMeatValue()));
    }

    private IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(deathAnimDur);
        enemyStats.ResetObjectData();
        gameObject.SetActive(false);
    }

    public void RefillHealth(float newCurrentHealth)
    {
        currentHealth = newCurrentHealth;
    }

    #endregion


    private void PlayDropSystemAnimation()
    {
        int dropRand = Random.Range(0, gameManager.dropTypeCount);
        OnDropAnimNeeded?.Invoke(dropRand);
    }

    private void SetObjectLayer(string newLayerName)
    {
        gameObject.layer = LayerMask.NameToLayer(newLayerName);
    }

    private void ChangeEnemyMat(Material newMat)
    {
        for (int i = 0; i < materialIndexes.Count; i++)
        {
            boundRenderer.materials[materialIndexes[i]] = newMat;
        }
    }

    #region  Getters & Setters
    public void SetIsDead(bool newIsDead)
    {
        isDead = newIsDead;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    #endregion
}
