using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : EnemyTarget
{
    UIManager uiManager;
    CityManager cityManager;
    PlayerStats playerStats;

    public Action OnPlayerKilled;

    public void Start()
    {
        uiManager = UIManager.instance;
        cityManager = FindObjectOfType<CityManager>();
        playerStats = GetComponent<PlayerStats>();
    }
    public void Kill()
    {
        gameObject.SetActive(false);
        Time.timeScale = 0.5f; // sonra balancelicaz

        // OnPlayerKilled?.Invoke();
        uiManager.HandleReviveUI();
    }
    // will be connected to revive button
    public void Revive()
    {
        transform.position = cityManager.GetRevivePoint().position;
        playerStats.FillCurrentHealth();
        gameObject.SetActive(true);

        uiManager.HandleReviveUI();
        Time.timeScale = 1;
    }
    public override void TakeDamage(float dmg)
    {
        playerStats.TakeDamage(dmg);
    }
}
