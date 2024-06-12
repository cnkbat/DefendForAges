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

    bool isBuyed;

    [Header("Tighted Buyable Area")]
    [SerializeField] BuyableArea tightedBuyableArea;

    [Header("Save & Load")]
    [Tooltip("Sadece save load için her bir obje için ayrı isimlendirme.")][SerializeField] public string buyableAreaID;

    [Header("Loadable")]
    [SerializeField] public LoadableBase loadableBase;

    [Header("Spawn Poses")]
    [SerializeField] private List<Transform> spawnPosesToEnable;
    [SerializeField] private List<Transform> spawnPosesToDisable;

    [Header("Visuals")]
    [SerializeField] private List<Transform> objectsToEnableOnBuy;
    [SerializeField] private List<Transform> objectsToDisableOnBuy;
    [SerializeField] private List<Transform> ghostedAssets;

    [Header("Events")]
    public Action OnAreaBuyed;
    public Action<int> OnAreaEnabled;

    public Action<List<Transform>, List<Transform>, List<Transform>> OnAnimPlayNeeded;

    #region OnEnable / OnDisable
    private void OnEnable()
    {
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


    }
    #endregion


    #region Buying Area
    public void AreaBuyed()
    {
        OnAreaBuyed?.Invoke();
        EnableArea();
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

        //    gameObject.SetActive(false);
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
                OnAnimPlayNeeded?.Invoke(objectsToEnableOnBuy, objectsToDisableOnBuy, spawnPosesToDisable); //spawnPosesToEnable eklenecek
            }
        }

        for (int i = 0; i < ghostedAssets.Count; i++)
        {
            ghostedAssets[i].gameObject.SetActive(!isBuyed);
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

    public bool GetIsBuyed()
    {
        return isBuyed;
    }

    #endregion
}
