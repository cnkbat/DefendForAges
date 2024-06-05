using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(SpawnAnimationHandler))]
public class BuyableArea : MonoBehaviour
{

    SaveManager saveManager;
    CityManager cityManager;
    NavMeshManager navMeshManager;
    bool isBuyed;

    [Header("Tighted Buyable Area")]
    [SerializeField] BuyableArea tightedBuyableArea;

    [Header("AI Related")]
    [SerializeField] private int surfaceAreaIndex;

    [Header("Save & Load")]
    [Tooltip("Sadece save load için her bir obje için ayrı isimlendirme.")][SerializeField] public string buyableAreaID;

    [Header("Loadable")]
    [SerializeField] public LoadableBase loadableBase;

    [Header("Spawn Poses")]
    [SerializeField] private List<Transform> spawnPosesToDisable;

    [Header("Visuals")]
    [SerializeField] private List<Transform> objectsToEnableOnBuy;
    [SerializeField] private List<Transform> objectsToDisableOnBuy;
    [SerializeField] private List<Transform> ghostedAssets;

    [Header("Events")]
    public Action OnAreaBuyed;
    public Action<int> OnAreaEnabled;
    public Action OnNavMeshUpdated;
    public Action<List<Transform>, List<Transform>, List<Transform>> OnAnimPlayNeeded;

    #region OnEnable / OnDisable
    private void OnEnable()
    {
        navMeshManager = NavMeshManager.instance;
        cityManager = transform.root.GetComponent<CityManager>();

        LoadBuyableAreaData();

        saveManager = SaveManager.instance;

        if (saveManager != null)
        {
            saveManager.OnSaved += SaveBuyableAreaData;
            saveManager.OnResetData += DeleteBuyableAreaData;
        }


        loadableBase.OnLoadableFilled += AreaBuyed;


        OnAreaBuyed += cityManager.AreaBuyed;
        OnAreaEnabled += cityManager.AreaEnabled;

        OnNavMeshUpdated += navMeshManager.BakeNavMesh;
    }

    private void OnDisable()
    {
        if (saveManager != null)
        {
            saveManager.OnSaved -= SaveBuyableAreaData;
            saveManager.OnResetData -= DeleteBuyableAreaData;
        }

        loadableBase.OnLoadableFilled -= AreaBuyed;

        OnAreaBuyed -= cityManager.AreaBuyed;
        OnAreaEnabled -= cityManager.AreaEnabled;

        OnNavMeshUpdated -= navMeshManager.BakeNavMesh;
    }
    #endregion


    #region Buying Area
    public void AreaBuyed()
    {
        Debug.Log("area buyed");
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
        if (isBuyed) return;

        if (tightedBuyableArea != null)
        {
            if (tightedBuyableArea.GetIsBuyed())
            {
                gameObject.SetActive(true);


                loadableBase.gameObject.SetActive(true);

                for (int i = 0; i < ghostedAssets.Count; i++)
                {
                    ghostedAssets[i].gameObject.SetActive(true);
                }
            }
        }
        else
        {

            gameObject.SetActive(true);

            if (isBuyed == false)
            {
                loadableBase.gameObject.SetActive(true);
            }


            for (int i = 0; i < ghostedAssets.Count; i++)
            {
                ghostedAssets[i].gameObject.SetActive(true);
            }
        }

    }

    public void DisableBuying()
    {
        if (isBuyed) return;

        gameObject.SetActive(false);
        loadableBase.gameObject.SetActive(false);
    }

    #endregion

    private void CheckForAssetsState()
    {
        loadableBase.gameObject.SetActive(!isBuyed);

        for (int i = 0; i < objectsToEnableOnBuy.Count; i++)
        {
            if (objectsToEnableOnBuy[i].gameObject.activeSelf != isBuyed)
            {
                OnAnimPlayNeeded?.Invoke(objectsToEnableOnBuy, objectsToDisableOnBuy, spawnPosesToDisable);
            }
        }

        for (int i = 0; i < ghostedAssets.Count; i++)
        {
            ghostedAssets[i].gameObject.SetActive(!isBuyed);
        }


        OnAreaEnabled?.Invoke(surfaceAreaIndex);
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
