using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    SaveManager saveManager;
    public StaticDefenceSO defenceSO;
    [SerializeField] protected LoadableBase loadableBase;

    [Header("Save & Load")]
    protected int upgradeIndex;

    [Header("Ingame Values")]
    protected float maxHealth;

    [Header("Events")]
    public Action OnBuyDone;

    protected virtual void OnEnable()
    {
        SetSOValues();
        saveManager = SaveManager.instance;
        loadableBase.OnLoadableFilled += BuyDone;
        saveManager.OnResetData += ResetData;
    }
    protected virtual void OnDisable()
    {
        loadableBase.OnLoadableFilled -= BuyDone;
        saveManager.OnResetData -= ResetData;
    }

    protected virtual void Start()
    {

    }

    public virtual void BuyDone()
    {
        OnBuyDone?.Invoke();
    }

    protected void IncrementUpgradeIndex()
    {
        upgradeIndex++;
        saveManager.OnSaved?.Invoke();
    }

    protected virtual void SetSOValues()
    {
        maxHealth = defenceSO.GetMaxHealthValues()[upgradeIndex];
    }

    #region Getters & Setters
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetUpgradeIndex()
    {
        return upgradeIndex;
    }
    public void SetUpgradeIndex(int newUpgradeIndex)
    {
        upgradeIndex = newUpgradeIndex;
    }

    public void SetLoadableBaseActivity(bool isActive)
    {
        loadableBase.gameObject.SetActive(isActive);
    }
    #endregion

    #region !! ADMIN !!
    protected virtual void ResetData()
    {
        upgradeIndex = 0;
        saveManager.OnSaved?.Invoke();
    }
    #endregion 
}

