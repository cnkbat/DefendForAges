﻿using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class EnemyBehaviour : MonoBehaviour, IPoolableObject, IDamagable
{
    //  saldırı, yürüme , ölme
    EnemyStats enemyStats;
    GameManager gameManager;
    EnemyTargeter enemyTargeter;
    PlayerStats playerStats;

    private EnemySpawner assignedEnemySpawner;

    [Header("Attacking")]
    public float attackDur;

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


    public void OnEnable()
    {
        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyStats = this.GetComponent<EnemyStats>();

        OnEnemySpawned += enemyTargeter.EnemySpawned;
        OnEnemySpawned += enemyStats.EnemySpawned;

        if (assignedEnemySpawner != null)
        {
            OnEnemyKilled += assignedEnemySpawner.OnEnemyKilled;
        }
    }

    public void OnDisable()
    {
        if (assignedEnemySpawner != null)
        {
            OnEnemyKilled -= assignedEnemySpawner.OnEnemyKilled;
        }
    }

    public void OnObjectPooled()
    {

        attackDur = 1;
        canMove = true;
        isDead = false;

        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        OnEnemySpawned?.Invoke();
    }


    public void Update()
    {
        if (isDead) return;
        if (!canMove) return;
        if (enemyTargeter.GetTarget() == null) return;

        Move();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ITargetable targetable))
        {
            Attack(targetable);
        }
    }


    #region  Movement
    IEnumerator EnableMovement(float duration)
    {

        yield return new WaitForSeconds(duration);

        if (isDead) yield return null;

        /* if (!isBoss)
         {
             animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 1);
             animator.SetBool("isMoving", true);
             animator.SetBool("hitTaken", false);
         } */

        canMove = true;
    }

    public void Move()
    {
        Vector3 targetPosition = enemyTargeter.GetTarget().position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, enemyStats.currentMoveSpeed * Time.deltaTime);
    }

    #endregion

    #region Combat Related
    public void Attack(ITargetable target)
    {
        canMove = false;
        // play animation
        // deal damage

        target.TakeDamage(enemyStats.GetDamage());
        StartCoroutine(EnableMovement(attackDur));

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

    public void Kill()
    {
        isDead = true;

        playerStats.OnKillEnemy.Invoke(enemyStats.GetMoneyValue(), enemyStats.GetExpValue(), enemyStats.GetPowerUpValue());

        OnEnemyKilled?.Invoke();
        gameManager.allSpawnedEnemies.Remove(gameObject);

        // test için commentlendi, geri getirilicek
        // enemyStats.getHealthBar().gameObject.SetActive(false);

        //GameObject spawnedFloatingText = ObjectPooler.instance.SpawnFromPool("Floating Text", floatingTextTransform.position);
        //spawnedFloatingText.GetComponent<FloatingTextAnimation>().SetText("$" + moneyValue.ToString());

        //gameObject.layer = LayerMask.NameToLayer("DeadZombie");


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
