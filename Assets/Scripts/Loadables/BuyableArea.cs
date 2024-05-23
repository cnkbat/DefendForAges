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
    [Tooltip("Sadece save load için her bir obje için ayrı isimlendirme.")][SerializeField] public string buyableAreaID;

    [Header("Loadable")]
    [SerializeField] public LoadableBase loadableBase;

    [Header("Enemy Spawns")]
    [SerializeField] List<Transform> enemySpawnAreas;

    [Header("Visuals")]
    // animatore set  edielecek
    [SerializeField] List<GameObject> assets;
    // animatore set  edielecek
    [SerializeField] List<GameObject> objectsToDisableOnBuy;
    [SerializeField] List<GameObject> ghostedAssets;

    private SpawnAnimationHandler spawnAnimationHandler;

    [Header("Events")]
    public Action OnAreaBuyed;
    public Action<List<Transform>> OnAreaEnabled;
    public Action OnNavMeshUpdated;

    #region OnEnable / OnDisable
    private void OnEnable()
    {
        navMeshManager = NavMeshManager.instance;
        cityManager = transform.parent.GetComponent<CityManager>();

        spawnAnimationHandler = GetComponentInChildren<SpawnAnimationHandler>();

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

                for (int i = 0; i < ghostedAssets.Count; i++)
                {
                    ghostedAssets[i].SetActive(true);
                }
            }
        }
        else
        {
            gameObject.SetActive(true);
            loadableBase.gameObject.SetActive(true);

            for (int i = 0; i < ghostedAssets.Count; i++)
            {
                ghostedAssets[i].SetActive(true);
            }
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

        for (int i = 0; i < assets.Count; i++)
        {
            if (assets[i].activeSelf != isBuyed)
            {
                assets[i].SetActive(isBuyed);
                // play activation animation
            }
        }

        for (int i = 0; i < ghostedAssets.Count; i++)
        {
            ghostedAssets[i].SetActive(!isBuyed);
        }


        spawnAnimationHandler?.OnAnimPlay?.Invoke();

        OnAreaEnabled?.Invoke(enemySpawnAreas);
        //OnNavMeshUpdated?.Invoke();
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
