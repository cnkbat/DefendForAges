using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using UnityEditor.Experimental;
using TMPro;

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

    [Header("Texts")]
    [SerializeField] private TMP_Text currentCostLeftForUpgradeText;
    [SerializeField] private TMP_Text repairText;

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
                UpdateCurrentMoneyText(currentCostLeftForUpgrade.ToString());
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


    private void UpdateCurrentMoneyText(string text)
    {
        currentCostLeftForUpgradeText.text = text;
    }

    #region Getters & Setters

    public void SetCurrentCostLeftForUpgrade(int value)
    {

        currentCostLeftForUpgrade = value;
        UpdateCurrentMoneyText(currentCostLeftForUpgrade.ToString());

        if (currentCostLeftForUpgrade > 0)
        {
            isFull = false;
        }
        else
        {
            isFull = true;
        }

    }

    #endregion

}

