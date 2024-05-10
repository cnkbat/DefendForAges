using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableArea : MonoBehaviour
{
    CityManager cityManager;
    bool isBuyed;

    [Header("Loadable")]
    [SerializeField] private LoadableBase loadableBase;

    [Header("Defences")]
    [SerializeField] List<DefencesStatsBase> defences;

    [Header("Enemy Spawns")]
    [SerializeField] List<Transform> enemySpawnAreas;

    [Header("Visuals")]
    [SerializeField] List<GameObject> assets;
    [SerializeField] List<GameObject> objectsToDisableOnBuy;

    [Header("Events")]
    public Action<List<Transform>> OnAreaBuyed;


    private void OnEnable()
    {
        loadableBase.OnLoadableFilled += AreaBuyed;
        cityManager = transform.parent.GetComponent<CityManager>();
        OnAreaBuyed = cityManager.AreaBuyed;

        // sonra bunu data saveye bağlicaz
        loadableBase.SetCurrentCostLeftForUpgrade(cityManager.buyableAreaCosts[cityManager.buyableAreas.IndexOf(this)]);
    }

    private void OnDisable()
    {
        loadableBase.OnLoadableFilled -= AreaBuyed;
    }

    private void Start()
    {
        CheckForAssetsState();
    }

    public void AreaBuyed()
    {
        SetIsBuyed(true);
        CheckForAssetsState();
        OnAreaBuyed?.Invoke(enemySpawnAreas);
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



    #region  Getters & Setters
    public void SetIsBuyed(bool newIsBuyed)
    {
        isBuyed = newIsBuyed;
    }
    #endregion
}
