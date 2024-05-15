using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental;

public class LoadableBase : MonoBehaviour
{
    GameManager gameManager;
    SaveManager saveManager;
    PlayerStats playerStats;
    protected ObjectPooler objectPooler;
    [SerializeField] private Transform moneyMovePos;
    public int currentCostLeftForUpgrade;

    [Header("State")]
    protected bool isFull;

    [Header("Events")]
    public Action OnLoadableFilled;
    private void OnEnable()
    {
        gameManager = GameManager.instance;

        gameManager.OnWaveStarted += DisableObject;
    }

    private void OnDisable()
    {
        gameManager.OnWaveStarted -= DisableObject;
    }
    
    protected virtual void Start()
    {
        objectPooler = ObjectPooler.instance;
        playerStats = PlayerStats.instance;
        saveManager = SaveManager.instance;
    }

    public virtual void Load()
    {

        if (isFull) return;

        if (currentCostLeftForUpgrade > 0)
        {
            if (playerStats.DecrementMoney(1))
            {
                currentCostLeftForUpgrade -= 1;
                saveManager.OnSaved?.Invoke();
            }
        }

        CheckIfFulled();
    }

    protected void CheckIfFulled()
    {
        if (currentCostLeftForUpgrade <= 0)
        {
            isFull = true;
            OnLoadableFilled?.Invoke();
        }
    }

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    #region Getters & Setters

    public void SetCurrentCostLeftForUpgrade(int value)
    {
        currentCostLeftForUpgrade = value;
    }

    #endregion

}

