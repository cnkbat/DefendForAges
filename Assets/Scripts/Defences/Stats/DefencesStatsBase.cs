using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    public StaticDefenceSO staticDefenceSO;
    [SerializeField] private int defencesID;
    protected float maxHealth;

    public void Start()
    {
        //  staticDefenceSO = Resources.Load<StaticDefenceSO>("DefenceSO"); Test kısmında okey ama hep elle yerleştirelim 
        GetSOValues();
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void GetSOValues()
    {
        maxHealth = staticDefenceSO.maxHealth;
    }
}
