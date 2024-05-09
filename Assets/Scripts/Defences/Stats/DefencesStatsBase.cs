using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    SaveManager saveManager;

    public StaticDefenceSO staticDefenceSO;
    [SerializeField] protected LoadableBase loadableBase;

    [Header("Save & Load")]
    int upgradeIndex;

    [Header("Events")]
    public Action OnBuyDone;

    protected virtual void OnEnable()
    {
        saveManager = SaveManager.instance;
        loadableBase.OnLoadableFilled += BuyDone;
        saveManager.OnResetData += ResetData;
    }
    protected virtual void OnDisable()
    {
        loadableBase.OnLoadableFilled -= BuyDone;
    }

    protected virtual void Start()
    {

    }

    public virtual void BuyDone()
    {
        OnBuyDone?.Invoke();
    }

    public void SetLoadableBaseActivity(bool isActive)
    {
        loadableBase.gameObject.SetActive(isActive);
    }

    protected void IncrementUpgradeIndex()
    {
        upgradeIndex++;
        saveManager.OnSaved?.Invoke();
    }


    #region Getters & Setters
    public float GetMaxHealth()
    {
        return staticDefenceSO.GetMaxHealthValues()[upgradeIndex];
    }
    public int GetUpgradeIndex()
    {
        return upgradeIndex;
    }
    public void SetUpgradeIndex(int newUpgradeIndex)
    {
        upgradeIndex = newUpgradeIndex;
    }
    #endregion

    #region !! ADMIN !!
    protected void ResetData()
    {
        upgradeIndex = 0;
        saveManager.OnSaved?.Invoke();
    }
    #endregion 
}

