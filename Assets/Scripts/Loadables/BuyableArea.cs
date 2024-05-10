using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableArea : MonoBehaviour
{
    SaveManager saveManager;
    CityManager cityManager;
    bool isBuyed;

    [Header("Save & Load")]
    [SerializeField] public int buyableAreaID;

    [Header("Loadable")]
    [SerializeField] public LoadableBase loadableBase;

    [Header("Defences")]
    [SerializeField] List<DefencesStatsBase> defences;

    [Header("Enemy Spawns")]
    [SerializeField] List<Transform> enemySpawnAreas;

    [Header("Visuals")]
    [SerializeField] List<GameObject> assets;
    [SerializeField] List<GameObject> objectsToDisableOnBuy;

    [Header("Events")]
    public Action OnAreaBuyed;
    public Action<List<Transform>> OnAreaEnabled;


    #region OnEnable / OnDisable
    private void OnEnable()
    {
        cityManager = transform.parent.GetComponent<CityManager>();
        LoadBuyableAreaData();


        saveManager = SaveManager.instance;
        saveManager.OnSaved += SaveBuyableAreaData;
        saveManager.OnResetData += DeleteBuyableAreaData;


        loadableBase.OnLoadableFilled += AreaBuyed;


        OnAreaBuyed += cityManager.AreaBuyed;
        OnAreaEnabled += cityManager.AreaEnabled;
    }

    private void OnDisable()
    {
        saveManager.OnSaved -= SaveBuyableAreaData;
        saveManager.OnResetData -= DeleteBuyableAreaData;

        loadableBase.OnLoadableFilled -= AreaBuyed;

        OnAreaBuyed -= cityManager.AreaBuyed;
        OnAreaEnabled -= cityManager.AreaEnabled;

    }
    #endregion

    private void Start()
    {
        CheckForAssetsState();
    }

    public void AreaBuyed()
    {
        EnableArea();
        OnAreaBuyed?.Invoke();
    }

    public void EnableArea()
    {
        SetIsBuyed(true);
        CheckForAssetsState();
        OnAreaEnabled?.Invoke(enemySpawnAreas);
    }

    private void CheckForAssetsState()
    {
        loadableBase.gameObject.SetActive(!isBuyed);

        for (int i = 0; i < objectsToDisableOnBuy.Count; i++)
        {
            objectsToDisableOnBuy[i].SetActive(!isBuyed);
        }

        for (int i = 0; i < assets.Count; i++)
        {
            if (assets[i].activeSelf != isBuyed)
            {
                assets[i].SetActive(isBuyed);
                // play activation animation
            }
        }
    }

    #region Save & Load
    public void SaveBuyableAreaData()
    {
        SaveSystem.SaveBuyableAreaData(this, buyableAreaID);
    }

    public void LoadBuyableAreaData()
    {

        BuyableAreaData buyableAreaData = SaveSystem.LoadBuyableAreaData(buyableAreaID);

        if (buyableAreaData != null)
        {
            loadableBase.SetCurrentCostLeftForUpgrade(buyableAreaData.currentCostLeftForUpgrade);
        }
        else
        {
            loadableBase.SetCurrentCostLeftForUpgrade(cityManager.buyableAreaCosts[cityManager.buyableAreas.IndexOf(this)]);
        }
    }

    public void DeleteBuyableAreaData()
    {
        SaveSystem.DeleteBuyableAreaData(buyableAreaID);
    }

    #endregion

    #region  Getters & Setters
    public void SetIsBuyed(bool newIsBuyed)
    {
        isBuyed = newIsBuyed;
    }
    #endregion
}
