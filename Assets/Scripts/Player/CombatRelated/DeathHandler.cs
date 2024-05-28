using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : EnemyTarget
{
    public Action OnDamageTaken;

    private void Awake()
    {
        isPlayer = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        playerStats.OnReviveButtonClicked += ReviveInstant;
        playerStats.OnLateReviveButtonClicked += LateRevive;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerStats.OnReviveButtonClicked -= ReviveInstant;
        playerStats.OnLateReviveButtonClicked -= LateRevive;
    }

    override protected void Start()
    {
        base.Start();
    }

    public void Kill()
    {
        if (isDestroyed) return;
        if (playerStats.GetIsDead()) return;

        Time.timeScale = 0.5f; // sonra balancelicaz

        playerStats.SetIsDead(true);
        isDestroyed = true;
        isTargetable = true;

        playerStats.OnPlayerKilled?.Invoke();
        OnTargetDestroyed?.Invoke();
    }

    // will be connected to revive button
    private void LateRevive()
    {
        RevivePlayer();

        transform.position = gameManager.allCities[playerStats.GetCityIndex()].GetRevivePoint().position;
        // put a timer here maybe?
    }

    // for the situation that player watches ads or something
    private void ReviveInstant()
    {
        RevivePlayer();
    }

    private void RevivePlayer()
    {
        playerStats.SetIsDead(false);
        isDestroyed = true;
        isTargetable = true;
        Time.timeScale = 1;

        playerStats?.OnPlayerRevived.Invoke();
    }

    public override void TakeDamage(float dmg)
    {
        if (playerStats.GetIsDead()) return;

        base.TakeDamage(dmg);

        OnDamageTaken?.Invoke();

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public bool IncrementCurrentHealth(float value)
    {
        if (currentHealth >= playerStats.GetMaxHealth())
        {
            return false;
        }
        else
        {
            currentHealth += value;

            if (currentHealth > playerStats.GetMaxHealth())
            {
                playerStats.FillCurrentHealth();
            }

            return true;
        }

    }

    #region Getters & Setters
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;
    }
    #endregion

}
