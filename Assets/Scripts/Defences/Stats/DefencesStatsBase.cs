using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    public StaticDefenceSO staticDefenceSO;
    [SerializeField] private int defencesID;

   
    protected virtual void Start()
    {
       //
    }

    public float GetMaxHealth()
    {
        return staticDefenceSO.maxHealth;
    }

}
