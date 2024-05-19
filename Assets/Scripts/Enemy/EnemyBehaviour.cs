using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IPoolableObject, IDamagable
{
    //  saldırı, yürüme , ölme

    #region City & Instances
    private GameManager gameManager;
    private PlayerStats playerStats;
    private EnemySpawner assignedEnemySpawner;
    private CityManager currentCityManager;

    #endregion

    [Header("Components on this")]
    private Rigidbody rb;
    private EnemyTargeter enemyTargeter;
    private EnemyStats enemyStats;
    private NavMeshAgent navMeshAgent;
    private EnemyAnimationHandler enemyAnimationHandler;

    [Header("AI Management")]
    [SerializeField] private float originalStoppingDistance;

    [Header("Combat")]
    private float attackTimer;

    [Header("Animation")]
    private Animator animator;

    [Header("States")]
    public bool isDead;
    private bool canMove;

    [Header("Layers")]
    [SerializeField] private string aliveLayerName;
    [SerializeField] private string deadLayerName;

    [Header("Animation")]
    [SerializeField] private float deathAnimDur;

    [Header("Health")]
    private float currentHealth;

    [Header("Events")]
    public Action OnEnemyKilled;
    public Action OnEnemySpawned;
    public Action OnAttacking;
    public Action OnTargetNotReached;
    public Action OnDeath;
    public Action OnMove;



    #region IPoolableObject Functions

    private void SetComponents()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        animator = this.GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody>();
        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyStats = this.GetComponent<EnemyStats>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimationHandler = GetComponent<EnemyAnimationHandler>();
    }

    public void OnObjectPooled()
    {
        SetComponents();

        canMove = true;
        isDead = false;
        animator.SetBool("isWalking", true);

        SetObjectLayer(aliveLayerName);

        OnEnemySpawned += enemyTargeter.EnemySpawned;
        OnEnemySpawned += enemyStats.EnemySpawned;
        OnEnemySpawned?.Invoke();

        currentCityManager = enemyTargeter.GetCityManager();
        assignedEnemySpawner = currentCityManager.GetCurrentWave();

        OnEnemyKilled += assignedEnemySpawner.OnEnemyKilled;

        navMeshAgent.stoppingDistance = originalStoppingDistance;

        RefillHealth(enemyStats.GetMaxHealth());
        ResetAttackSpeed();
    }



    public void ResetObjectData()
    {
        OnEnemySpawned -= enemyTargeter.EnemySpawned;
        OnEnemySpawned -= enemyStats.EnemySpawned;
        OnEnemyKilled -= assignedEnemySpawner.OnEnemyKilled;


        navMeshAgent.isStopped = false;

        animator.SetBool("isKill", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", false);

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

    private void LookAtTarget()
    {
        Vector3 worldAimTarget = enemyTargeter.GetTarget().transform.position;
        worldAimTarget.y = transform.position.y;
    }

    private void Attacking()
    {

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, enemyStats.GetRange()))
        {
            if (hit.transform.TryGetComponent(out EnemyTarget enemyTarget))
            {
                navMeshAgent.stoppingDistance = enemyTarget.GetStoppingDistance();
                attackTimer -= Time.deltaTime;
                canMove = false;

                if (attackTimer <= 0)
                {
                    // Invoke attacking
                    OnAttacking?.Invoke();
                    //animator.SetTrigger("Attacking");
                    ResetAttackSpeed();
                }
            }
            else
            {
                OnTargetNotReached?.Invoke();
                //animator.SetBool("isAttacking", false);
                canMove = true;
                navMeshAgent.stoppingDistance = originalStoppingDistance;
            }
        }
        else
        {
            OnTargetNotReached?.Invoke();
            //animator.SetBool("isAttacking", false);
            canMove = true;
            navMeshAgent.stoppingDistance = originalStoppingDistance;
        }

    }



    #region  Movement
    IEnumerator EnableMovement(float duration)
    {
        if (isDead) yield return null;

        yield return new WaitForSeconds(duration);

        if (isDead) yield return null;

        canMove = true;
    }

    public void Move()
    {
        navMeshAgent.isStopped = !canMove;
        OnMove?.Invoke();
        //animator.SetBool("isWalking", canMove);

        navMeshAgent.destination = enemyTargeter.GetTarget().transform.position;
    }

    #endregion

    #region Combat Related
    public void DealDamage(ITargetable target)
    {

        // play animation
        // deal damage
        target.TakeDamage(enemyStats.GetDamage());
    }

    public void TakeDamage(float dmg)
    {

        currentHealth -= dmg;

        canMove = false;

        if (currentHealth <= 0)
        {
            if (isDead) return;
            Kill();
        }

        StartCoroutine(EnableMovement(enemyStats.GetKnockbackDuration()));
    }

    public void ResetAttackSpeed()
    {
        attackTimer = enemyStats.GetAttackSpeed();
    }

    public void Kill()
    {
        isDead = true;
        canMove = false;

        playerStats.OnKillEnemy.Invoke(enemyStats.GetMoneyValue(), enemyStats.GetExpValue(), enemyStats.GetMeatValue(), enemyStats.GetPowerUpValue());

        gameManager.allSpawnedEnemies.Remove(gameObject);

        OnDeath?.Invoke();
        SetObjectLayer(deadLayerName);

        OnEnemyKilled?.Invoke();
        StartCoroutine(KillEnemy());

        // test için commentlendi, geri getirilicek
        // enemyStats.getHealthBar().gameObject.SetActive(false);

        //GameObject spawnedFloatingText = ObjectPooler.instance.SpawnFromPool("Floating Text", floatingTextTransform.position);
        //spawnedFloatingText.GetComponent<FloatingTextAnimation>().SetText("$" + moneyValue.ToString());

        //gameObject.layer = LayerMask.NameToLayer("DeadZombie");
    }


    public void RefillHealth(float newCurrentHealth)
    {
        currentHealth = newCurrentHealth;
    }

    #endregion

    private IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(deathAnimDur);
        ResetObjectData();
        gameObject.SetActive(false);
    }

    #region  Getters & Setters

    public bool GetCanMove()
    {
        return canMove;
    }

    private void SetObjectLayer(string newLayerName)
    {
        gameObject.layer = LayerMask.NameToLayer(newLayerName);
    }

    #endregion
}
