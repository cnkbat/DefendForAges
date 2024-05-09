using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class LoadableBase : MonoBehaviour
{
    SaveManager saveManager;
    PlayerStats playerStats;
    protected ObjectPooler objectPooler;
    [SerializeField] private Transform moneyMovePos;

    [Header("Costs")]
    [SerializeField] protected List<int> costs;
    int costIndex;
    public int currentCostLeftForUpgrade;

    [Header("State")]
    protected bool isFull;

    [Header("Events")]
    public Action OnLoadableFilled;
    public Action OnLoadableTookMoney;

    private void OnEnable()
    {

        saveManager = SaveManager.instance;
        saveManager.OnSaved += SaveData;

    }

    protected virtual void Start()
    {

        objectPooler = ObjectPooler.instance;
        playerStats = PlayerStats.instance;
        //  UpdateCurrentCostLeft();

        // Data load Save

        CheckIfFulled();
    }

    public virtual void Load()
    {
        if (isFull) return;

        currentCostLeftForUpgrade -= 1;
        playerStats.DecrementMoney(1);

        OnLoadableTookMoney?.Invoke();
        CheckIfFulled();
    }

    public void UpdateCurrentCostLeft()
    {
        currentCostLeftForUpgrade = costs[costIndex];
    }

    protected void CheckIfFulled()
    {

        if (currentCostLeftForUpgrade <= 0)
        {
            isFull = true;
            OnLoadableFilled?.Invoke();
        }
    }

    protected virtual void SaveData()
    {
        // data save;
    }

    #region Getters & Setters

    public void SetCost(List<int> newCosts)
    {
        costs = newCosts;
    }

    public void SetCost(int newCost)
    {
        costs[0] = newCost;
    }

    public void SetCostIndex(int index)
    {
        costIndex = index;
    }

    #endregion

}

