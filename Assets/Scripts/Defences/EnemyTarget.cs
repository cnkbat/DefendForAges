using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ITargetable
{
    private GameManager gameManager;
    protected PlayerStats playerStats;

    protected float currentHealth;
    public Action OnTargetDestroyed;
    private void OnEnable()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        OnTargetDestroyed += gameManager.allCities[playerStats.GetCurrentCityIndex()].UpdateTargetList;
    }
    private void OnDisable()
    {
        OnTargetDestroyed -= gameManager.allCities[playerStats.GetCurrentCityIndex()].UpdateTargetList;
    }

    /// fiziksel hasar
    /// yıkım ölüm
    /// vs.

    public virtual void TakeDamage(float dmg)
    {
        throw new System.NotImplementedException();
        // chlidlara atamak için
    }

    public virtual void ResetHealthValue()
    {
        // childlara atamak için
    }

    public Transform GetTarget()
    {
        return transform;
    }


}
