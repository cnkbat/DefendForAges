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
    [SerializeField] private float enableRecoveryTimer;
    [SerializeField] private float recoveryTimer = 2;
    private float currentEnableRecoveryTimer;
    private float currentRecoveryTimer;
    bool isRecoveryActive;

    [Header("Events")]
    public Action OnTowerDestroyed;
    public Action OnRecoveryDone;

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
        if (isDestroyed) return;
        if(gameManager.isGameFreezed) return;
        
        base.Update();

        if (currentHealth >= towerStats.GetMaxHealth())
        {
            return;
        }

        if (isRecoveryActive)
        {
            recoveryTimer -= Time.deltaTime;

            if (recoveryTimer < 0)
            {
                currentHealth += towerStats.GetRecovery();
                OnRecoveryDone?.Invoke();
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

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);

        ResetEnableRecoveryTimer();
        // haptic oynat
        // feeli ver.
    }

    private void ResetEnableRecoveryTimer()
    {
        isRecoveryActive = false;
        currentEnableRecoveryTimer = enableRecoveryTimer;
    }

    private void ResetRecoveryTimer()
    {
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
        upgradeUIHandler.transform.DOScale(4, 1f).SetEase(Ease.OutElastic);
    }

    public void DisableUpgrader()
    {
        upgradeUIHandler.transform.DOScale(0, 1f).SetEase(Ease.InElastic).OnComplete(() => upgradeUIHandler.gameObject.SetActive(false));

    }


}
