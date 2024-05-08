using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    public StaticDefenceSO staticDefenceSO;
    [SerializeField] protected LoadableBase loadableBase;

    [Header("Save & Load")]
    int upgradeIndex;

    [Header("Events")]
    public Action OnBuyDone;

    protected virtual void OnEnable()
    {
        loadableBase.OnLoadableFilled += BuyDone;
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
}

