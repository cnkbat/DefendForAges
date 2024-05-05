using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ITargetable
{
    private GameManager gameManager;
    protected PlayerStats playerStats;
    protected float currentHealth;
    public Action OnTargetDestroyed;

    protected virtual void OnEnable()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;
        OnTargetDestroyed += gameManager.allCities[playerStats.GetCurrentCityIndex()].UpdateTargetList;
    }
    protected virtual void OnDisable()
    {

    }

    protected virtual void Start()
    {

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

    public virtual void TargetDestroyed()
    {
        OnTargetDestroyed?.Invoke();
        OnTargetDestroyed -= gameManager.allCities[playerStats.GetCurrentCityIndex()].UpdateTargetList;
    }
    public Transform GetTarget()
    {
        return transform;
    }


}
