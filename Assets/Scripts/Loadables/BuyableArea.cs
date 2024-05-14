using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BuyableArea : MonoBehaviour
{
    SaveManager saveManager;
    CityManager cityManager;
    NavMeshManager navMeshManager;
    bool isBuyed;

    [Header("Tighted Buyable Area")]
    [SerializeField] BuyableArea tightedBuyableArea;

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
    public Action OnNavMeshUpdated;

    #region OnEnable / OnDisable
    private void OnEnable()
    {
        navMeshManager = NavMeshManager.instance;

        cityManager = transform.parent.GetComponent<CityManager>();
        LoadBuyableAreaData();


        saveManager = SaveManager.instance;
        saveManager.OnSaved += SaveBuyableAreaData;
        saveManager.OnResetData += DeleteBuyableAreaData;


        loadableBase.OnLoadableFilled += AreaBuyed;


        OnAreaBuyed += cityManager.AreaBuyed;
        OnAreaEnabled += cityManager.AreaEnabled;

        OnNavMeshUpdated += navMeshManager.BakeNavMesh;

    }

    private void OnDisable()
    {
        saveManager.OnSaved -= SaveBuyableAreaData;
        saveManager.OnResetData -= DeleteBuyableAreaData;

        loadableBase.OnLoadableFilled -= AreaBuyed;

        OnAreaBuyed -= cityManager.AreaBuyed;
        OnAreaEnabled -= cityManager.AreaEnabled;

        OnNavMeshUpdated -= navMeshManager.BakeNavMesh;
    }
    #endregion

    private void Start()
    {
        CheckForAssetsState();
    }

    #region Buying Area
    public void AreaBuyed()
    {
        EnableArea();
        OnAreaBuyed?.Invoke();
    }

    public void EnableArea()
    {
        SetIsBuyed(true);
        CheckForAssetsState();
    }

    public void EnableBuying()
    {
        if (tightedBuyableArea != null)
        {
            if (tightedBuyableArea.GetIsBuyed())
            {
                gameObject.SetActive(true);
                loadableBase.gameObject.SetActive(true);
            }
        }
        else
        {
            gameObject.SetActive(true);
            loadableBase.gameObject.SetActive(true);
        }

    }

    public void DisableBuying()
    {
        if (isBuyed) return;

        gameObject.SetActive(false);
        loadableBase.gameObject.SetActive(true);
    }
    #endregion

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

        OnAreaEnabled?.Invoke(enemySpawnAreas);
        OnNavMeshUpdated?.Invoke();
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

    public bool GetIsBuyed()
    {
        return isBuyed;
    }

    #endregion
}
