using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencesStatsBase : MonoBehaviour
{
    [SerializeField] private int defencesID;
    protected float maxHealth;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
