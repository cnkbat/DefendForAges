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
    EnemyStats enemyStats;
    GameManager gameManager;
    EnemyTargeter enemyTargeter;
    PlayerStats playerStats;

    CityManager cityManager;
    private EnemySpawner assignedEnemySpawner;
    private Rigidbody rb;
    private float attackTimer;
    [SerializeField] float range;
    private Animator anim;

    [Header("States")]
    public bool isDead;
    public bool canMove;
    Animator animator;
    [Header("Animation")]
    [SerializeField] float deathAnimDur;

    [Header("Health")]
    float currentHealth;

    [Header("Events")]
    public Action OnEnemyKilled;
    public Action OnEnemySpawned;

    private NavMeshAgent navMeshAgent;

    #region IPoolableObject Functions


    public void OnObjectPooled()
    {

        canMove = true;
        isDead = false;
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        anim = this.GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody>();
        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyStats = this.GetComponent<EnemyStats>();

        OnEnemySpawned += enemyTargeter.EnemySpawned;
        OnEnemySpawned += enemyStats.EnemySpawned;


        cityManager = FindObjectOfType<CityManager>();
        ConnectToSpawner();

        OnEnemySpawned?.Invoke();

        navMeshAgent = GetComponent<NavMeshAgent>();
        RefillHealth(enemyStats.GetMaxHealth());
        ResetAttackSpeed();
        range = 5f;

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
        Vector3 endPoint = new Vector3(startPoint.x + (transform.TransformDirection(Vector3.forward) * range).x, startPoint.y + (transform.TransformDirection(Vector3.forward) * range).y, startPoint.z + (transform.TransformDirection(Vector3.forward) * range).z);
        if (Physics.Raycast(startPoint, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range) && (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") || hit.transform.gameObject.layer == LayerMask.NameToLayer("Defence")))
        {
            //anim.SetTrigger("Attack");
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                anim.SetTrigger("Attack");
                ResetAttackSpeed();
            }
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
        Vector3 worldAimTarget = enemyTargeter.GetTarget().transform.position;
        worldAimTarget.y = transform.position.y;

        navMeshAgent.destination = worldAimTarget;
        //Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        //transform.forward = aimDirection;

        ////Vector3 targetPosition = enemyTargeter.GetTarget().position;
        //// transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyStats.currentMoveSpeed * Time.deltaTime);

        //transform.position += transform.forward * enemyStats.currentMoveSpeed * Time.deltaTime;
    }

    #endregion

    #region Combat Related
    public void Attack(ITargetable target)
    {

        canMove = false;
        // play animation
        // deal damage
        target.TakeDamage(enemyStats.GetDamage());
        StartCoroutine(EnableMovement(enemyStats.attackDur));

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
        attackTimer = playerStats.GetAttackSpeed();
    }

    public void Kill()
    {
        isDead = true;
        canMove = false;

        playerStats.OnKillEnemy.Invoke(enemyStats.GetMoneyValue(), enemyStats.GetExpValue(), enemyStats.GetMeatValue(), enemyStats.GetPowerUpValue());

        OnEnemyKilled?.Invoke();
        gameManager.allSpawnedEnemies.Remove(gameObject);
        Debug.Log("killed");

        anim.SetTrigger("Kill");


        // bu ikisi ölüm animasyonundan sonra runlamalı
        //gameObject.SetActive(false);
        //ResetObjectData();

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
