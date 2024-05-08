using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableArea : MonoBehaviour
{
    bool isBuyed;

    [SerializeField] private LoadableBase loadableBase;

    private void OnEnable() 
    {
        loadableBase.OnLoadableFilled += AreaBuyed;
    }

    private void OnDisable() 
    {
        loadableBase.OnLoadableFilled -= AreaBuyed;
    }

    void Update()
    {
        
    }

    public void AreaBuyed()
    {
        isBuyed = true;
        
    }
}
