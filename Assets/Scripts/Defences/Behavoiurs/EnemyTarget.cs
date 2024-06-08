using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyTarget : MonoBehaviour, ITargetable
{
    protected GameManager gameManager;
    protected CityManager cityManager;
    protected PlayerStats playerStats;

    protected float currentHealth;
    protected bool isTargetable;
    [SerializeField] public bool isPlayer;
    protected bool isDestroyed;
    public Action OnDamageTaken;

    [Header("AI Manipulation")]
    [SerializeField] private float stoppingDistance;
    [SerializeField] private List<Transform> targetPoints;

    [Header("Events")]
    public Action OnTargetDestroyed;
    public Action OnTargetRevived;
    private void Awake()
    {
        if (!isPlayer)
        {
            cityManager = transform.root.GetComponent<CityManager>();
        }
    }

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
        if (isDestroyed) return;

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
        if (!isTargetable) return null;

        int randIndex = Random.Range(0, targetPoints.Count);
        return targetPoints[randIndex];
    }

    public bool GetIsTargetable()
    {
        return isTargetable;
    }

    public float GetStoppingDistance()
    {
        return stoppingDistance;
    }

    public bool GetIsDestroyed()
    {
        return isDestroyed;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public CityManager GetCityManager()
    {
        return cityManager;
    }
    #endregion

}
