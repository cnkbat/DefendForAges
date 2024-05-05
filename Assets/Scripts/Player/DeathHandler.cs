using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : EnemyTarget
{
    UIManager uiManager;
    CityManager cityManager;
    public Action OnPlayerKilled;

    protected override void OnEnable()
    {
        base.OnEnable();
        playerStats.OnRevive += Revive;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        playerStats.OnRevive -= Revive;
    }

    override protected void Start()
    {
        base.Start();

        uiManager = UIManager.instance;
        cityManager = FindObjectOfType<CityManager>();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        Time.timeScale = 0.5f; // sonra balancelicaz

        OnTargetDestroyed?.Invoke();
        uiManager.HandleReviveUI();
    }

    // will be connected to revive button
    public void Revive()
    {
        transform.position = cityManager.GetRevivePoint().position;
        gameObject.SetActive(true);

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
