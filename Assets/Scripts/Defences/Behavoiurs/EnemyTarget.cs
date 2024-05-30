using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ITargetable
{
    protected GameManager gameManager;
    protected CityManager cityManager;

    protected PlayerStats playerStats;

    protected float currentHealth;
    protected bool isTargetable;
    protected bool isPlayer;

    protected bool isDestroyed;

    [Header("AI Manipulation")]
    [SerializeField] private float stoppingDistance;

    [Header("Events")]
    public Action OnTargetDestroyed;
    public Action OnTargetRevived;

    protected virtual void OnEnable()
    {
        playerStats = PlayerStats.instance;
        gameManager = GameManager.instance;

        if (isPlayer)
        {
            OnTargetDestroyed += gameManager.allCities[playerStats.GetCityIndex()].UpdateTargetList;
            OnTargetRevived += gameManager.allCities[playerStats.GetCityIndex()].UpdateTargetList;
        }
        else
        {
            cityManager = transform.root.GetComponent<CityManager>();
            OnTargetDestroyed += cityManager.UpdateTargetList;
            OnTargetRevived += cityManager.UpdateTargetList;

            cityManager.UpdateTargetList();
        }

        isTargetable = true;
    }
    protected virtual void OnDisable()
    {
        if (isPlayer)
        {
            OnTargetDestroyed -= gameManager.allCities[playerStats.GetCityIndex()].UpdateTargetList;
            OnTargetRevived -= gameManager.allCities[playerStats.GetCityIndex()].UpdateTargetList;
        }
        else
        {
            OnTargetDestroyed -= transform.root.GetComponent<CityManager>().UpdateTargetList;
            OnTargetRevived -= transform.root.GetComponent<CityManager>().UpdateTargetList;
        }
    }

    protected virtual void Start()
    {

    }

    /// fiziksel hasar
    /// yıkım ölüm
    /// vs.

    public virtual void TakeDamage(float dmg)
    {
        if (isDestroyed) return;

        // chlidlara atamak için
    }

    public virtual void ResetHealthValue()
    {
        // childlara atamak için
    }

    public virtual void TargetDestroyed()
    {
        isDestroyed = true;
        isTargetable = false;

        OnTargetDestroyed?.Invoke();
    }

    public virtual void TargetRevived()
    {
        isDestroyed = false;
        isTargetable = true;

        OnTargetRevived?.Invoke();
    }

    #region Getters & Setters
    public Transform GetTarget()
    {
        return transform;
    }

    public bool GetIsTargetable()
    {
        return isTargetable;
    }

    public float GetStoppingDistance()
    {
        return stoppingDistance;
    }

    #endregion

}
