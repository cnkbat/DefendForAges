using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{

    GameManager gameManager;
    EnemyBehaviour enemyBehaviour;
    NavMeshAgent navMeshAgent;

    public EnemySO enemySO;

    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float maxHealth;

    [Tooltip("Attacking")]
    private float attackSpeed;
    private float attackDur;
    private float knockbackDur;
    private float currentDamage;
    private float attackRange;
    private float movementSpeed;

    [Header("Earnings")]
    private int moneyValue;
    private int expValue;
    private int meatValue;
    private float powerUpAddOnValue;

    [Header("Health Bar")]
    [SerializeField] Slider healthBar;

    public void EnemySpawned()
    {
        gameManager = GameManager.instance;
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetEnemySOValues();
    }

    private void SetEnemySOValues()
    {

        powerUpAddOnValue = enemySO.GetPowerUpAddOnValue();
        knockbackDur = enemySO.GetKnockbackDur();
        currentMoveSpeed = enemySO.GetMovementSpeed();
        currentDamage = enemySO.GetDamage();
        moneyValue = enemySO.GetMoneyValue();
        expValue = enemySO.GetExpValue();
        meatValue = enemySO.GetMeatValue();
        maxHealth = enemySO.GetMaxHealth();
        movementSpeed = enemySO.GetMovementSpeed();
        attackDur = enemySO.GetAttackDur();
        attackRange = enemySO.GetAttackRange();

        navMeshAgent.speed = movementSpeed;

    }

    #region Health Related

    /*  private void UpdateHealthBar()
      {

          if (!healthBar) return;
          healthBar.gameObject.SetActive(true);

          if (currentHealth < maxHealth)
          {
              for (int i = 0; i < healthBarImages.Count; i++)
              {
                  healthBarImages[i].DOFade(255f, 0);
              }

              healthBar.value = currentHealth / maxHealth;

              for (int i = 0; i < healthBarImages.Count; i++)
              {
                  healthBarImages[i].DOFade(0, 1);
              }
          }


      } */

    #endregion

    #region Getters & Setters

    #region Combats
    public float GetDamage()
    {
        return currentDamage;
    }
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetAttackDur()
    {
        return attackDur;
    }

    public float GetRange()
    {
        return attackRange;
    }

    #endregion

    #region Earnings
    public int GetMoneyValue()
    {
        return moneyValue;
    }
    public int GetExpValue()
    {
        return expValue;
    }
    public int GetMeatValue()
    {
        return meatValue;
    }
    public float GetPowerUpValue()
    {
        return powerUpAddOnValue;
    }
    #endregion 

    #region Movement
    public float GetKnockbackDuration()
    {
        return knockbackDur;
    }
    #endregion


    #endregion

}
