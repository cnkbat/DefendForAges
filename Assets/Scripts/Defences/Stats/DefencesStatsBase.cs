using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    public StaticDefenceSO staticDefenceSO;
    [SerializeField] protected LoadableBase loadableBase;
    [SerializeField] private int defencesID;

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
        // chlid atama
    }

    public virtual void BuyDone()
    {
        OnBuyDone?.Invoke();
    }

    public void SetLoadableBaseActivity(bool isActive)
    {
        loadableBase.gameObject.SetActive(isActive);
    }

    public float GetMaxHealth()
    {
        return staticDefenceSO.maxHealth;
    }

}

