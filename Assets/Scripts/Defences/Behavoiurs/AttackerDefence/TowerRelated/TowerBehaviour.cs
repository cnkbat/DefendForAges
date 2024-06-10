using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : AttackerDefenceBehaviour
{
    NearestEnemyFinder nearestEnemyFinder;
    TowerStats towerStats;

    UpgradeUIHandler upgradeUIHandler;

    [Header("Events")]
    public Action OnTowerDestroyed;

    [Header("Recovery")]
    [SerializeField] private float recoveryTimer;
    private float currentRecoveryTimer;

    bool isRecoveryActive;

    private void Awake()
    {
        upgradeUIHandler = GetComponentInChildren<UpgradeUIHandler>();
        towerStats = GetComponent<TowerStats>();
    }
    override protected void OnEnable()
    {
        base.OnEnable();

        for (int i = 0; i < towerStats.GetWeapons().Count; i++)
        {
            OnRangedAttack += towerStats.GetWeapons()[i].Attack;
        }


        gameManager.OnWaveStarted += DisableUpgrader;
        playerStats.OnWaveWon += EnableUpgrader;

        ResetRecoveryTimer();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        for (int i = 0; i < towerStats.GetWeapons().Count; i++)
        {
            OnRangedAttack -= towerStats.GetWeapons()[i].Attack;
        }
    }

    override protected void Start()
    {
        base.Start();

        nearestEnemyFinder = GetComponent<NearestEnemyFinder>();
        ResetHealthValue();
    }

    protected override void Update()
    {
        base.Update();


        if (isRecoveryActive)
        {
            if (currentHealth <= towerStats.GetMaxHealth())
            {
                currentHealth += towerStats.GetRecovery() * Time.deltaTime;
            }
        }
        else
        {
            currentRecoveryTimer -= Time.deltaTime;

            if (currentRecoveryTimer <= 0)
            {
                isRecoveryActive = true;
            }
        }

    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        ResetRecoveryTimer();
        // haptic oynat
        // feeli ver.
    }

    private void ResetRecoveryTimer()
    {
        isRecoveryActive = false;
        currentRecoveryTimer = recoveryTimer;
    }

    protected override void Attack()
    {
        base.Attack();

        if (!nearestEnemyFinder.GetNearestEnemy()) return;

        OnRangedAttack?.Invoke(nearestEnemyFinder.GetNearestEnemy(), towerStats.GetDamage(), false);
    }

    protected override void DestroyDefence()
    {
        if (isDestroyed) return;

        base.DestroyDefence();
        // ağır haptic oynat
        OnTowerDestroyed?.Invoke();
    }



    public void EnableUpgrader()
    {
        upgradeUIHandler.gameObject.SetActive(true);
    }

    public void DisableUpgrader()
    {
        upgradeUIHandler.gameObject.SetActive(false);
    }


}
