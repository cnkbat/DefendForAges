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
    private CityManager cityManager;
    private EnemySpawner assignedEnemySpawner;

    #endregion

    [Header("Components on this")]
    private Rigidbody rb;
    private EnemyTargeter enemyTargeter;
    private EnemyStats enemyStats;
    private NavMeshAgent navMeshAgent;

    [Header("AI Management")]
    [SerializeField] private float originalStoppingDistance;

    [Header("Combat")]
    private float attackTimer;

    [Header("Animation")]
    private Animator animator;

    [Header("States")]
    public bool isDead;
    public bool canMove;

    [Header("Animation")]
    [SerializeField] private float deathAnimDur;

    [Header("Health")]
    private float currentHealth;

    [Header("Events")]
    public Action OnEnemyKilled;
    public Action OnEnemySpawned;


    #region IPoolableObject Functions

    public void OnObjectPooled()
    {

        canMove = true;
        isDead = false;
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        animator = this.GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody>();
        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyStats = this.GetComponent<EnemyStats>();

        OnEnemySpawned += enemyTargeter.EnemySpawned;
        OnEnemySpawned += enemyStats.EnemySpawned;

        cityManager = FindObjectOfType<CityManager>();
        ConnectToSpawner();

        OnEnemySpawned?.Invoke();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = originalStoppingDistance;

        RefillHealth(enemyStats.GetMaxHealth());
        ResetAttackSpeed();

    }

    public void ResetObjectData()
    {

        OnEnemySpawned -= enemyTargeter.EnemySpawned;
        OnEnemySpawned -= enemyStats.EnemySpawned;

        cityManager = FindObjectOfType<CityManager>();
        cityManager.OnWaveCalled -= ConnectToSpawner;
        OnEnemyKilled -= assignedEnemySpawner.OnEnemyKilled;

    }

    #endregion

    public void Update()
    {
        rb.velocity = Vector3.zero;
        if (isDead) return;
        if (!canMove) return;
        if (enemyTargeter.GetTarget() == null) return;

        Attacking();
        Move();
    }

    private void Attacking()
    {
        Vector3 startPoint = transform.position;

        if (Physics.Raycast(startPoint, transform.TransformDirection(Vector3.forward), out RaycastHit hit, enemyStats.GetRange()))
        {
            if (hit.transform.TryGetComponent(out EnemyTarget enemyTarget))
            {

                navMeshAgent.stoppingDistance = enemyTarget.GetStoppingDistance();

                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    animator.SetTrigger("Attack");
                    ResetAttackSpeed();
                }
            }
            else
            {
                navMeshAgent.stoppingDistance = originalStoppingDistance;
            }
        }
        else
        {
            navMeshAgent.stoppingDistance = originalStoppingDistance;

        }
    }

    public void ConnectToSpawner()
    {
        assignedEnemySpawner = FindObjectOfType<EnemySpawner>();
        OnEnemyKilled += assignedEnemySpawner.OnEnemyKilled;
    }

    #region  Movement
    IEnumerator EnableMovement(float duration)
    {

        yield return new WaitForSeconds(duration);

        if (isDead) yield return null;

        canMove = true;
    }

    public void Move()
    {
        navMeshAgent.isStopped = !canMove;

        Vector3 worldAimTarget = enemyTargeter.GetTarget().transform.position;
        worldAimTarget.y = transform.position.y;

        navMeshAgent.destination = enemyTargeter.GetTarget().transform.position;
    }

    #endregion

    #region Combat Related
    public void DealDamage(ITargetable target)
    {

        // play animation
        // deal damage
        target.TakeDamage(enemyStats.GetDamage());
        StartCoroutine(EnableMovement(enemyStats.GetAttackDur()));

    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        canMove = false;
        StartCoroutine(EnableMovement(enemyStats.GetKnockbackDuration()));

        if (currentHealth <= 0)
        {
            Kill();
        }

    }

    private void ResetAttackSpeed()
    {
        attackTimer = enemyStats.GetAttackSpeed();
    }

    public void Kill()
    {
        isDead = true;
        canMove = false;

        playerStats.OnKillEnemy.Invoke(enemyStats.GetMoneyValue(), enemyStats.GetExpValue(), enemyStats.GetMeatValue(), enemyStats.GetPowerUpValue());

        OnEnemyKilled?.Invoke();
        gameManager.allSpawnedEnemies.Remove(gameObject);
        animator.SetTrigger("Kill");

        DestroyObject();
        ResetObjectData();

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

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(deathAnimDur);
        gameObject.SetActive(false);
    }


    #region  Getters & Setters
    public void SetEnemySpawner(EnemySpawner newEnemySpawner)
    {
        assignedEnemySpawner = newEnemySpawner;
    }
    #endregion
}
