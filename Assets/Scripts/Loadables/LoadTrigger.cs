using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour, ILoadable
{
    LoadableBase parentObj;

    private void Start() 
    {
        if(transform.parent.TryGetComponent(out LoadableBase loadableBase))
        {
            parentObj = loadableBase;
        }    
    }
    
    public void Load()
    {
        parentObj.Load();
    }

    public void UnLoad()
    {
        parentObj.UnLoad();
    }

}
