using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

public class EnemyStats : MonoBehaviour, IPoolableObject
{

    public EnemySO enemySO;

    [Header("Components On This")]
    private EnemyAttack enemyAttack;
    private EnemyTargeter enemyTargeter;
    private EnemyDeathHandler enemyDeathHandler;
    private EnemyMovement enemyMovement;
    private NavMeshAgent navMeshAgent;

    [Header("Cities & Instances")]
    private CityManager currentCityManager;
    [HideInInspector] public EnemySpawner assignedEnemySpawner;


    [Header("Values")]
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
    private float coinValue;
    private float expValue;
    private float meatValue;
    private float powerUpAddOnValue;

    [Header("Events")]
    public Action OnEnemySpawned;

    public Action OnDataReset;

    private void Awake()
    {
        enemyTargeter = this.GetComponent<EnemyTargeter>();
        enemyMovement = this.GetComponent<EnemyMovement>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        enemyDeathHandler = this.GetComponent<EnemyDeathHandler>();
        enemyAttack = this.GetComponent<EnemyAttack>();
    }
    public void OnObjectPooled()
    {
        SetEnemySOValues();
        
        OnEnemySpawned += enemyTargeter.EnemySpawned;
        OnEnemySpawned += enemyMovement.EnemySpawned;
        OnEnemySpawned += enemyAttack.EnemySpawned;


        OnEnemySpawned?.Invoke();

        currentCityManager = enemyTargeter.GetCityManager();
        assignedEnemySpawner = currentCityManager.GetCurrentWave();

        enemyDeathHandler.EnemySpawned();
        OnDataReset += enemyDeathHandler.ResetData;


    }

    public void ResetObjectData()
    {
        OnEnemySpawned -= enemyTargeter.EnemySpawned;
        OnEnemySpawned -= enemyMovement.EnemySpawned;
        OnEnemySpawned -= enemyAttack.EnemySpawned;

        OnDataReset?.Invoke();
        OnDataReset -= enemyDeathHandler.ResetData;

        navMeshAgent.isStopped = false;

    }

    private void SetEnemySOValues()
    {

        powerUpAddOnValue = enemySO.GetPowerUpAddOnValue();
        knockbackDur = enemySO.GetKnockbackDur();
        currentMoveSpeed = enemySO.GetMovementSpeed();
        currentDamage = enemySO.GetDamage();
        coinValue = enemySO.GetMoneyValue();
        expValue = enemySO.GetExpValue();
        meatValue = enemySO.GetMeatValue();
        maxHealth = enemySO.GetMaxHealth();
        movementSpeed = enemySO.GetMovementSpeed();
        attackDur = enemySO.GetAttackDur();
        attackRange = enemySO.GetAttackRange();
        attackSpeed = enemySO.GetAttackSpeed();

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

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }
    #endregion

    #region Earnings
    public float GetMoneyValue()
    {
        return coinValue;
    }
    public float GetExpValue()
    {
        return expValue;
    }
    public float GetMeatValue()
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

    public EnemySpawner GetAssignedEnemySpawner()
    {
        return assignedEnemySpawner;
    }

    #endregion

}
