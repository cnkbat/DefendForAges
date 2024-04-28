using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadableBase : MonoBehaviour
{
    protected ObjectPooler objectPooler;

    [Header("Load Values & Needs")]
    [SerializeField] protected float loadTimer;
    [SerializeField] protected Image fillImage;
    [SerializeField] protected GameObject fillTrigger;
    [SerializeField] protected ParticleSystem openupVFX;

    protected virtual void Start()
    {

        objectPooler = ObjectPooler.instance;
    }

    public virtual void Load()
    {


        fillImage.fillAmount += Time.deltaTime / loadTimer;

        if (fillImage.fillAmount >= 1)
        {
            OpenUp();
        }
    }

    public virtual void UnLoad()
    {
        if (fillImage.fillAmount > 0)
        {

            fillImage.fillAmount = 0;

        }
    }

    protected virtual void OpenUp()
    {
        if (openupVFX)
        {
            openupVFX.Play();
        }


        /// vfx çıkacak
        /// vermesi gereken şeyleri verecek ama alt child behaviorda
    }

    public virtual void ResetLoadable()
    {
        Debug.Log(gameObject.name);

        fillImage.fillAmount = 0;

    }
}

