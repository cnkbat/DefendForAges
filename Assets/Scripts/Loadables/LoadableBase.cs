using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadableBase : MonoBehaviour
{
    protected ObjectPooler objectPooler;

    protected virtual void Start()
    {
        objectPooler = ObjectPooler.instance;
    }

    public virtual void Load()
    {

    }

    public virtual void ResetLoadable()
    {
        
    }
}

