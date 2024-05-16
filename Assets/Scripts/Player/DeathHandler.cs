using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : EnemyTarget
{
    UIManager uiManager;
    CityManager cityManager;
    public Action OnPlayerKilled;

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

        uiManager = UIManager.instance;
        cityManager = FindObjectOfType<CityManager>();
    }

    public void Kill()
    {
        Time.timeScale = 0.5f; // sonra balancelicaz

        OnTargetDestroyed?.Invoke();
        uiManager.HandleReviveUI();
    }

    // will be connected to revive button
    public void LateRevive()
    {
        transform.position = cityManager.GetRevivePoint().position;
        // put a timer here maybe?

        uiManager.HandleReviveUI();
        Time.timeScale = 1;
    }
    // for the situation that player watches ads or something
    public void ReviveInstant()
    {

        uiManager.HandleReviveUI();
        Time.timeScale = 1;
    }
    public override void TakeDamage(float dmg)
    {

        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void IncerementCurrentHealth(float value)
    {
        currentHealth += value;

        if (currentHealth > playerStats.GetMaxHealth())
        {
            playerStats.FillCurrentHealth();
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
