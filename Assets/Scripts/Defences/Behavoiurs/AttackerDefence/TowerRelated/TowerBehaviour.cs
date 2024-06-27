using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TowerBehaviour : AttackerDefenceBehaviour
{
    NearestEnemyFinder nearestEnemyFinder;
    TowerStats towerStats;

    UpgradeUIHandler upgradeUIHandler;

    [Header("Recovery")]
    [Tooltip("Vurulduktan kaç saniye sonra başlayacak")][SerializeField] private float enableRecoveryTimer;
    [Tooltip("Kaç saniyede bir canı artacak")][SerializeField] private float recoveryTimer = 2;
    private float currentEnableRecoveryTimer;
    private float currentRecoveryTimer;
    bool isRecoveryActive;

    [Header("Events")]
    public Action OnTowerDestroyed;
    public Action OnRecoveryDone;
    public Action OnHealthFilled;
    public Action OnTowerRevived;

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

        ResetEnableRecoveryTimer();
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(5);
        }

        if (isDestroyed) return;
        if (gameManager.isGameFreezed) return;

        base.Update();

        if (currentHealth >= towerStats.GetMaxHealth())
        {
            ResetEnableRecoveryTimer();
            return;
        }

        if (isRecoveryActive)
        {
            currentRecoveryTimer -= Time.deltaTime;

            if (currentRecoveryTimer < 0)
            {
                IncrementHealth();
                ResetRecoveryTimer();
            }
        }
        else
        {
            currentEnableRecoveryTimer -= Time.deltaTime;

            if (currentEnableRecoveryTimer <= 0)
            {
                isRecoveryActive = true;
            }
        }

    }

    #region Base Class Func
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        ResetEnableRecoveryTimer();
        // haptic oynat
        // feeli ver.
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


    public override void ReviveTarget()
    {
        isRepairable = true;
        base.ReviveTarget();
        ResetHealthValue();

        OnTowerRevived?.Invoke();

        List<EnemyDeathHandler> targets = new List<EnemyDeathHandler>(gameManager.allSpawnedEnemies);
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].Kill();
        }

    }

    public override void ResetHealthValue()
    {
        base.ResetHealthValue();
        OnHealthFilled?.Invoke();
    }

    #endregion

    #region Recovery
    private void ResetEnableRecoveryTimer()
    {
        isRecoveryActive = false;
        currentEnableRecoveryTimer = enableRecoveryTimer;
    }

    private void ResetRecoveryTimer()
    {
        currentRecoveryTimer = recoveryTimer;
    }

    private void IncrementHealth()
    {
        currentHealth += towerStats.GetRecovery();

        if (currentHealth >= towerStats.GetMaxHealth())
        {
            ResetHealthValue();
        }
        else
        {
            OnRecoveryDone?.Invoke();
        }
    }

    #endregion

    #region  Upgrader

    public void EnableUpgrader()
    {
        upgradeUIHandler.gameObject.SetActive(true);
        upgradeUIHandler.transform.DOScale(4, 1f).SetEase(Ease.OutElastic);
    }

    public void DisableUpgrader()
    {
        upgradeUIHandler.transform.DOScale(0, 1f).SetEase(Ease.InElastic).OnComplete(() => upgradeUIHandler.gameObject.SetActive(false));

    }

    #endregion

}
