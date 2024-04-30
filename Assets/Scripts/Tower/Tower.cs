using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    PlayerStats playerStats;
    private float maxHealth;
    private float currentHealth;
    [SerializeField] private float recovery;

    void Start()
    {
        playerStats = PlayerStats.instance;
        //maxHealth = ;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
