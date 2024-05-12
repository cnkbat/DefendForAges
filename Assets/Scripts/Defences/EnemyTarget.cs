using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyTarget : MonoBehaviour, ITargetable
{
    protected GameManager gameManager;
    protected PlayerStats playerStats;
    protected float currentHealth;
    protected bool isTargetable;
    protected bool isPlayer;


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
            OnTargetDestroyed += transform.root.GetComponent<CityManager>().UpdateTargetList;
            OnTargetRevived += transform.root.GetComponent<CityManager>().UpdateTargetList;
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
        throw new System.NotImplementedException();
        // chlidlara atamak için
    }

    public virtual void ResetHealthValue()
    {
        // childlara atamak için
    }

    public virtual void TargetDestroyed()
    {
        isTargetable = false;
        OnTargetDestroyed?.Invoke();
    }

    public virtual void TargetRevived()
    {
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
