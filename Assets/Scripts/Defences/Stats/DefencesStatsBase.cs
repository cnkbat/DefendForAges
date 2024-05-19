﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    [Header("Instances")]
    SaveManager saveManager;

    [Header("Components On This")]

    private DefencesBehaviourBase defencesBehaviourBase;
    public StaticDefenceSO defenceSO;
    [SerializeField] public LoadableBase loadableBase;

    [Header("Save & Load")]
    public int defenceID;
    public int upgradeIndex;

    [Header("Ingame Values")]
    protected float maxHealth;

    [Header("Events")]
    public Action OnBuyDone;

    protected virtual void OnEnable()
    {
        saveManager = SaveManager.instance;
        defencesBehaviourBase = GetComponent<DefencesBehaviourBase>();

        defencesBehaviourBase.OnRepairStateChange += loadableBase.SetIsRepairNeeded;
        loadableBase.OnRepairDone += defencesBehaviourBase.TargetRevived;

        loadableBase.OnLoadableFilled += BuyDone;
        loadableBase.OnLoadableFilled += SaveDefenceData;

        saveManager.OnSaved += SaveDefenceData;
        saveManager.OnResetData += ResetData;

        LoadDefenceData();
    }
    protected virtual void OnDisable()
    {
        
        defencesBehaviourBase.OnRepairStateChange -= loadableBase.SetIsRepairNeeded;
        loadableBase.OnRepairDone -= defencesBehaviourBase.TargetRevived;

        loadableBase.OnLoadableFilled -= BuyDone;
        loadableBase.OnLoadableFilled -= SaveDefenceData;

        saveManager.OnSaved -= SaveDefenceData;
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


    #region Save & Load

    public void SaveDefenceData()
    {
        SaveSystem.SaveDefencesData(this, defenceID);
    }

    protected virtual void LoadDefenceData()
    {
        DefencesData defencesData = SaveSystem.LoadDefenceData(defenceID);

        if (defencesData != null)
        {
            this.upgradeIndex = defencesData.upgradeIndex;
            SetSOValues();
            this.loadableBase.SetCurrentCostLeftForUpgrade(defencesData.currentCostLeftForUpgrade);
        }
        else
        {
            SetSOValues();
        }
    }

    #endregion

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
    public virtual void ResetData()
    {
        SaveSystem.DeleteDefencesData(defenceID);
    }
    #endregion 
}

