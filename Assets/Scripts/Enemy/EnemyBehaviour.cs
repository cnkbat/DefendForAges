using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using MoreMountains.Feedbacks;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour, IPoolableObject, IDamagable
{
    //  saldırı, yürüme , ölme

    #region City & Instances

    private GameManager gameManager;
    private PlayerStats playerStats;
    private EarningsHolder earningsHolder;
    private EnemySpawner assignedEnemySpawner;
    private CityManager currentCityManager;

    #endregion

    [Header("Components on this")]
    private Rigidbody rb;
    private EnemyTargeter enemyTargeter;
    private EnemyStats enemyStats;
    private NavMeshAgent navMeshAgent;
    private MMFeedbacks feelFeedBacks;

    [Header("AI Management")]
    [SerializeField] private float originalStoppingDistance;

    [Header("Combat")]
    private float attackTimer;

    [Header("States")]
    public bool isDead;
    [SerializeField] private bool canMove;

    [Header("Layers")]
    [SerializeField] private string aliveLayerName;
    [SerializeField] private string deadLayerName;

    [Header("Health")]
    private float currentHealth;

    #region Visuals

    [Header("*--- Visuals ---*")]

    [Header("Animation")]

    [SerializeField] private float deathAnimDur;
    [SerializeField] private Transform enemyAsset;
    [SerializeField] private float aimSpeed = 20f;

    [Header("Color Change On Death")]
    [SerializeField] private Renderer boundRenderer;
    [SerializeField] private List<int> materialIndexes = new List<int>();
    [SerializeField] private Material originalMat;
    [SerializeField] private Material deadMat;

    #endregion

    #region Events

    [Header("State Events")]
    public Action OnEnemySpawned;
    public Action OnDeath;

    [Header("Combat Related Events")]
    public Action OnAttacking;
    public Action OnDamageTaken;

    [Header("Movement Related Events")]
    public Action OnTargetNotReached;
    public Action OnMove;

    [Header("Stat Change Related Events")]
    public Action<float> OnMovementSpeedChanged;
    public Action<float> OnAttackSpeedChanged;

    [Header("Visual Events")]
    public Action<int> OnDropAnimNeeded;

    #endregion

    #region IPoolableObject Functions

    private void SetComponents()
    {
        playerStats = PlayerStats.instance;
        earningsHolder = EarningsHolder.instance;
        gameManager = GameManager.instance;

        rb = this.GetComponent<Rigidbody>();
        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyStats = this.GetComponent<EnemyStats>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        feelFeedBacks = GetComponentInChildren<MMFeedbacks>();
        enemyAsset = transform.Find("enemyAsset");

        if (feelFeedBacks != null)
        {
            feelFeedBacks.Initialization();
        }
    }

    public void OnObjectPooled()
    {
        SetComponents();

        canMove = true;
        isDead = false;

        OnMove?.Invoke();

        SetObjectLayer(aliveLayerName);

        OnEnemySpawned += enemyTargeter.EnemySpawned;
        OnEnemySpawned += enemyStats.EnemySpawned;
        OnEnemySpawned?.Invoke();

        currentCityManager = enemyTargeter.GetCityManager();
        assignedEnemySpawner = currentCityManager.GetCurrentWave();

        OnDeath += assignedEnemySpawner.OnEnemyKilled;

        navMeshAgent.stoppingDistance = originalStoppingDistance;

        ChangeEnemyMat(originalMat);

        RefillHealth(enemyStats.GetMaxHealth());
        ResetAttackSpeed();

    }



    public void ResetObjectData()
    {
        OnEnemySpawned -= enemyTargeter.EnemySpawned;
        OnEnemySpawned -= enemyStats.EnemySpawned;
        OnDeath -= assignedEnemySpawner.OnEnemyKilled;


        navMeshAgent.isStopped = false;

    }

    #endregion

    public void Update()
    {
        rb.velocity = Vector3.zero;

        if (isDead) return;

        if (enemyTargeter.GetTarget() == null) return;

        LookAtTarget();
        Attacking();

        Move();
    }

    #region Attacking

    private void LookAtTarget()
    {
        Vector3 worldAimTarget = enemyTargeter.GetTarget().transform.position;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, aimSpeed * Time.deltaTime);

        enemyAsset.forward = transform.forward;

        enemyAsset.localPosition = Vector3.zero;
    }

    private void Attacking()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("OnlyPlayer");

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, enemyStats.GetRange(), layerMask))
        {
            if (hit.transform.TryGetComponent(out EnemyTarget enemyTarget))
            {
                navMeshAgent.stoppingDistance = enemyTarget.GetStoppingDistance();
                attackTimer -= Time.deltaTime;
                canMove = false;

                if (attackTimer <= 0)
                {
                    OnAttacking?.Invoke();
                    ResetAttackSpeed();
                }
            }
            else
            {
                OnTargetNotReached?.Invoke();
                canMove = true;
                navMeshAgent.stoppingDistance = originalStoppingDistance;
            }
        }
        else
        {
            OnTargetNotReached?.Invoke();
            canMove = true;
            navMeshAgent.stoppingDistance = originalStoppingDistance;
        }

    }

    public void DealDamage(ITargetable target)
    {

        // play animation
        // deal damage
        target.TakeDamage(enemyStats.GetDamage());
    }

    public void ResetAttackSpeed()
    {
        attackTimer = enemyStats.GetAttackSpeed();
        OnAttackSpeedChanged?.Invoke(enemyStats.GetAttackSpeed());
    }

    #endregion

    #region  Movement
    IEnumerator EnableMovement(float duration)
    {
        if (isDead) yield return null;

        yield return new WaitForSeconds(duration);

        if (isDead) yield return null;

        navMeshAgent.speed = enemyStats.GetMovementSpeed();
    }

    public void Move()
    {
        navMeshAgent.isStopped = !canMove;
        OnMove?.Invoke();

        OnMovementSpeedChanged?.Invoke(navMeshAgent.speed);
        navMeshAgent.destination = enemyTargeter.GetTarget().transform.position;
    }

    #endregion

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

        StartCoroutine(EnableMovement(enemyStats.GetKnockbackDuration()));

    }

    public void Kill()
    {
        isDead = true;
        canMove = false;
        navMeshAgent.speed = 0;
        OnMovementSpeedChanged.Invoke(0);

        ChangeEnemyMat(deadMat);

        ApplyPlayerEarnings();

        gameManager.allSpawnedEnemies.Remove(gameObject);

        PlayDropSystemAnimation();

        OnDeath?.Invoke();
        SetObjectLayer(deadLayerName);

        StartCoroutine(KillEnemy());

        // test için commentlendi, geri getirilicek
        // enemyStats.getHealthBar().gameObject.SetActive(false);

        //GameObject spawnedFloatingText = ObjectPooler.instance.SpawnFromPool("Floating Text", floatingTextTransform.position);
        //spawnedFloatingText.GetComponent<FloatingTextAnimation>().SetText("$" + moneyValue.ToString());

        //gameObject.layer = LayerMask.NameToLayer("DeadZombie");
    }

    private void ApplyPlayerEarnings()
    {
        playerStats.OnKillEnemy?.Invoke(enemyStats.GetPowerUpValue());
        earningsHolder.OnTempEarningsUpdated?.Invoke(enemyStats.GetMoneyValue(), enemyStats.GetExpValue(), enemyStats.GetMeatValue());
    }

    private IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(deathAnimDur);
        ResetObjectData();
        gameObject.SetActive(false);
    }

    public void RefillHealth(float newCurrentHealth)
    {
        currentHealth = newCurrentHealth;
    }

    #endregion

    #region Visual Functions

    private void ChangeEnemyMat(Material newMat)
    {
        for (int i = 0; i < materialIndexes.Count; i++)
        {
            boundRenderer.materials[materialIndexes[i]] = newMat;
        }
    }

    
    private void PlayDropSystemAnimation()
    {
        int dropRand = Random.Range(0, gameManager.dropTypeCount);
        OnDropAnimNeeded?.Invoke(dropRand);
    }

    #endregion

    #region  Getters & Setters

    public bool GetCanMove()
    {
        return canMove;
    }

    private void SetObjectLayer(string newLayerName)
    {
        gameObject.layer = LayerMask.NameToLayer(newLayerName);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    #endregion

}
